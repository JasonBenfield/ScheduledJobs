using XTI_Core;
using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfJobDb : IJobDb
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public EfJobDb(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents)
    {
        foreach (var registeredEvent in registeredEvents)
        {
            var evtDefEntity = await db.EventDefinitions.Retrieve()
                .FirstOrDefaultAsync(ed => ed.EventKey == registeredEvent.EventKey.Value);
            if (evtDefEntity == null)
            {
                evtDefEntity = new EventDefinitionEntity
                {
                    EventKey = registeredEvent.EventKey.Value,
                    DisplayText = registeredEvent.EventKey.DisplayText,
                    CompareSourceKeyAndDataForDuplication = registeredEvent.CompareSourceKeyAndDataForDuplication,
                    DuplicateHandling = registeredEvent.DuplicateHandling,
                    TimeToStartNotifications = registeredEvent.TimeToStartNotifications,
                    ActiveFor = registeredEvent.ActiveFor,
                    DeleteAfter = registeredEvent.DeleteAfter
                };
                await db.EventDefinitions.Create(evtDefEntity);
            }
        }
    }

    public async Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs)
    {
        foreach (var registeredJob in registeredJobs)
        {
            var jobDefEntity = await AddOrUpdateJobDefinition(registeredJob);
            foreach (var task in registeredJob.Tasks)
            {
                await AddOrUpdateTaskDefinition(jobDefEntity, task);
            }
        }
    }

    private async Task<JobDefinitionEntity> AddOrUpdateJobDefinition(RegisteredJob registeredJob)
    {
        var jobDefEntity = await db.JobDefinitions.Retrieve()
            .FirstOrDefaultAsync(jd => jd.JobKey == registeredJob.JobKey.Value);
        if (jobDefEntity == null)
        {
            jobDefEntity = new JobDefinitionEntity
            {
                JobKey = registeredJob.JobKey.Value,
                DisplayText = registeredJob.JobKey.DisplayText,
                Timeout = registeredJob.Timeout,
                DeleteAfter = registeredJob.DeleteAfter
            };
            await db.JobDefinitions.Create(jobDefEntity);
        }
        else
        {
            await db.JobDefinitions.Update
            (
                jobDefEntity,
                jd => jd.Timeout = registeredJob.Timeout
            );
        }
        return jobDefEntity;
    }

    private async Task AddOrUpdateTaskDefinition(JobDefinitionEntity jobDefinitionEntity, RegisteredJobTask task)
    {
        var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
            .FirstOrDefaultAsync(td => td.TaskKey == task.TaskKey.Value);
        if (taskDefEntity == null)
        {
            taskDefEntity = new JobTaskDefinitionEntity
            {
                JobDefinitionID = jobDefinitionEntity.ID,
                TaskKey = task.TaskKey.Value,
                DisplayText = task.TaskKey.DisplayText,
                Timeout = task.Timeout
            };
            await db.JobTaskDefinitions.Create(taskDefEntity);
        }
        else
        {
            await db.JobTaskDefinitions.Update
            (
                taskDefEntity,
                td => td.Timeout = task.Timeout
            );
        }
    }

    public async Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, EventSource[] sources)
    {
        var eventNotifications = new List<EventNotificationModel>();
        var eventDefinition = await db.EventDefinitions.Retrieve()
            .FirstOrDefaultAsync(ed => ed.EventKey == eventKey.Value);
        if (eventDefinition == null)
        {
            throw new ArgumentException($"Event '{eventKey.DisplayText}' not found");
        }
        var now = clock.Now();
        if (now >= eventDefinition.TimeToStartNotifications)
        {
            var duplicateHandling = DuplicateHandling.Values.Value(eventDefinition.DuplicateHandling);
            foreach (var source in sources)
            {
                var duplicateNotifications = await GetDuplicateNotifications(eventDefinition, duplicateHandling, source);
                var timeActive = now;
                var activeFor = eventDefinition.ActiveFor;
                var timeInactive = activeFor == TimeSpan.MaxValue
                    ? DateTimeOffset.MaxValue
                    : now.Add(activeFor);
                if (duplicateHandling.Equals(DuplicateHandling.Values.KeepOldest) && duplicateNotifications.Any())
                {
                    timeActive = DateTimeOffset.MaxValue;
                    timeInactive = now;
                }
                if (duplicateHandling.Equals(DuplicateHandling.Values.KeepNewest))
                {
                    await DeactiveDuplicateEventNotifications(now, duplicateNotifications);
                }
                if (!duplicateHandling.Equals(DuplicateHandling.Values.Ignore) || !duplicateNotifications.Any())
                {
                    var notificationEntity = new EventNotificationEntity
                    {
                        EventDefinitionID = eventDefinition.ID,
                        SourceKey = source.SourceKey,
                        SourceData = source.SourceData,
                        TimeAdded = now,
                        TimeActive = timeActive,
                        TimeInactive = timeInactive,
                        TimeToDelete = now.Add(eventDefinition.DeleteAfter)
                    };
                    await db.EventNotifications.Create(notificationEntity);
                    eventNotifications.Add(new EventNotificationModel(notificationEntity.ID));
                }
            }
        }
        return eventNotifications.ToArray();
    }

    private async Task<EventNotificationEntity[]> GetDuplicateNotifications(EventDefinitionEntity eventDefinition, DuplicateHandling duplicateHandling, EventSource source)
    {
        EventNotificationEntity[] duplicateNotifications;
        if (duplicateHandling.Equals(DuplicateHandling.Values.KeepAll))
        {
            duplicateNotifications = new EventNotificationEntity[0];
        }
        else
        {
            var duplicateNotificationsQuery = db.EventNotifications.Retrieve()
                .Where
                (
                    en =>
                        en.EventDefinitionID == eventDefinition.ID
                        && en.SourceKey == source.SourceKey
                );
            if (eventDefinition.CompareSourceKeyAndDataForDuplication)
            {
                duplicateNotificationsQuery = duplicateNotificationsQuery
                    .Where
                    (
                        en => en.SourceData == source.SourceData
                    );
            }
            duplicateNotifications = await duplicateNotificationsQuery.ToArrayAsync();
        }
        return duplicateNotifications;
    }


    private async Task DeactiveDuplicateEventNotifications(DateTimeOffset now, EventNotificationEntity[] duplicateNotifications)
    {
        foreach (var duplicateNotification in duplicateNotifications)
        {
            await db.EventNotifications.Update
            (
                duplicateNotification,
                dn => dn.TimeInactive = now.AddMinutes(-1)
            );
        }
    }

    public async Task<TriggeredJobDetailModel[]> RetryJobs(JobKey jobKey)
    {
        var jobDefinitionID = db.JobDefinitions.Retrieve()
            .Where(jd => jd.JobKey == jobKey.Value)
            .Select(jd => jd.ID);
        var triggeredJobIDs = db.TriggeredJobs.Retrieve()
            .Where(tj => jobDefinitionID.Contains(tj.JobDefinitionID))
            .Select(tj => tj.ID);
        var now = clock.Now();
        var retryTasks = await db.TriggeredJobTasks.Retrieve()
            .Where
            (
                t =>
                    triggeredJobIDs.Contains(t.TriggeredJobID) &&
                    t.Status == JobTaskStatus.Values.Retry.Value &&
                    now >= t.TimeActive
            )
            .ToArrayAsync();
        foreach (var retryTask in retryTasks)
        {
            await db.TriggeredJobTasks.Update
            (
                retryTask,
                t => t.Status = JobTaskStatus.Values.Pending.Value
            );
        }
        var pendingJobIDs = db.TriggeredJobTasks.Retrieve()
            .GroupBy(jt => jt.TriggeredJobID)
            .Select(grouped => new { JobID = grouped.Key, Status = grouped.Min(t => t.Status) })
            .Where(grouped => grouped.Status == JobTaskStatus.Values.Pending.Value)
            .Select(grouped => grouped.JobID);
        var triggeredJobs = await
            db.TriggeredJobs.Retrieve()
                .Where(tj => pendingJobIDs.Contains(tj.ID))
                .Join
                (
                    db.JobDefinitions.Retrieve(),
                    tj => tj.JobDefinitionID,
                    jd => jd.ID,
                    (tj, jd) => new TriggeredJobWithDefinitionEntity(tj, jd)
                )
                .ToArrayAsync();
        var triggeredJobModels = new List<TriggeredJobDetailModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    public async Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime)
    {
        var jobDefinitionEntity = await db.JobDefinitions.Retrieve()
            .FirstOrDefaultAsync(jd => jd.JobKey == jobKey.Value);
        if (jobDefinitionEntity == null)
        {
            throw new ArgumentException($"Job '{jobKey.DisplayText}' was not found");
        }
        var eventDefinitionID = db.EventDefinitions.Retrieve()
            .Where(ed => ed.EventKey == eventKey.Value)
            .Select(ed => ed.ID);
        var triggeredJobEventNotificationIDs = db.TriggeredJobs.Retrieve()
            .Where(tj => tj.JobDefinitionID == jobDefinitionEntity.ID)
            .Select(tj => tj.EventNotificationID);
        var now = clock.Now();
        var eventNotifications = await db.EventNotifications.Retrieve()
            .Where
            (
                en =>
                    eventDefinitionID.Contains(en.EventDefinitionID)
                    && !triggeredJobEventNotificationIDs.Contains(en.ID)
                    && now >= en.TimeActive
                    && now < en.TimeInactive
                    && en.TimeAdded >= eventRaisedStartTime
            )
            .ToArrayAsync();
        var pendingJobs = new List<PendingJobModel>();
        foreach (var eventNotification in eventNotifications)
        {
            var triggeredJobEntity = new TriggeredJobEntity
            {
                EventNotificationID = eventNotification.ID,
                JobDefinitionID = jobDefinitionEntity.ID
            };
            await db.TriggeredJobs.Create(triggeredJobEntity);
            var triggeredJob = CreateTriggeredJobDetailModel
            (
                triggeredJobEntity, 
                jobDefinitionEntity, 
                new TriggeredJobTaskModel[0]
            );
            var pendingJob = new PendingJobModel(triggeredJob.Job, eventNotification.SourceData);
            pendingJobs.Add(pendingJob);
        }
        return pendingJobs.ToArray();
    }

    public async Task<TriggeredJobDetailModel[]> TriggeredJobs(int notificationID)
    {
        var triggeredJobs = await
            db.TriggeredJobs.Retrieve()
                .Where(tj => tj.EventNotificationID == notificationID)
                .Join
                (
                    db.JobDefinitions.Retrieve(),
                    tj => tj.JobDefinitionID,
                    jd => jd.ID,
                    (tj, jd) => new TriggeredJobWithDefinitionEntity(tj, jd)
                )
                .ToArrayAsync();
        var triggeredJobModels = new List<TriggeredJobDetailModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    private sealed record TriggeredJobWithDefinitionEntity(TriggeredJobEntity Job, JobDefinitionEntity Definition);

    private async Task<TriggeredJobDetailModel> GetTriggeredJob(int jobID)
    {
        var jobWithDef = await
            db.TriggeredJobs.Retrieve()
                .Where(tj => tj.ID == jobID)
                .Join
                (
                    db.JobDefinitions.Retrieve(),
                    tj => tj.JobDefinitionID,
                    jd => jd.ID,
                    (tj, jd) => new TriggeredJobWithDefinitionEntity(tj, jd)
                )
                .FirstAsync();
        var jobModel = await GetTriggeredJob(jobWithDef);
        return jobModel;
    }

    private async Task<TriggeredJobDetailModel> GetTriggeredJob(TriggeredJobWithDefinitionEntity jobWithDef)
    {
        var tasks = await TaskModels(jobWithDef.Job.ID);
        var jobModel = CreateTriggeredJobDetailModel(jobWithDef.Job, jobWithDef.Definition, tasks);
        return jobModel;
    }

    private async Task<TriggeredJobTaskModel[]> TaskModels(int jobID)
    {
        var taskModels = new List<TriggeredJobTaskModel>();
        var taskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID)
            .Join
            (
                db.JobTaskDefinitions.Retrieve(),
                t => t.TaskDefinitionID,
                td => td.ID,
                (t, td) => new { Task = t, Definition = td }
            )
            .OrderBy(grouped => grouped.Task.Sequence)
            .ToArrayAsync();
        foreach (var t in taskEntities)
        {
            var entries = await db.LogEntries.Retrieve()
                .Where(e => e.TaskID == t.Task.ID)
                .ToArrayAsync();
            taskModels.Add(CreateTriggeredJobTaskModel(t.Definition, t.Task, entries));
        }
        return taskModels.ToArray();
    }

    private static TriggeredJobDetailModel CreateTriggeredJobDetailModel(TriggeredJobEntity jobEntity, JobDefinitionEntity jobDefEntity, TriggeredJobTaskModel[] tasks) =>
        new TriggeredJobDetailModel
        (
            new TriggeredJobModel
            (
                jobEntity.ID,
                new JobDefinitionModel
                (
                    jobDefEntity.ID,
                    new JobKey(jobDefEntity.DisplayText)
                )
            ),
            tasks
        );

    public async Task<TriggeredJobDetailModel> StartJob(int jobID, NextTaskModel[] nextTasks)
    {
        await db.Transaction
        (
            async () =>
            {
                var jobWithDefEntity = await db.TriggeredJobs.Retrieve()
                    .Where(j => j.ID == jobID)
                    .Join
                    (
                        db.JobDefinitions.Retrieve(),
                        j => j.JobDefinitionID,
                        jd => jd.ID,
                        (j, jd) => new TriggeredJobWithDefinitionEntity(j, jd)
                    )
                    .FirstAsync();
                var now = clock.Now();
                await db.TriggeredJobs.Update
                (
                    jobWithDefEntity.Job,
                    j =>
                    {
                        j.TimeInactive = now.Add(jobWithDefEntity.Definition.Timeout);
                        j.TimeToDelete = now.Add(jobWithDefEntity.Definition.DeleteAfter);
                    }
                );
                await AddNextTasks(jobWithDefEntity.Job, null, nextTasks, now);
            }
        );
        var updatedJob = await GetTriggeredJob(jobID);
        return updatedJob;
    }

    public async Task StartTask(int taskID)
    {
        var taskEntity = await db.TriggeredJobTasks.Retrieve()
            .FirstOrDefaultAsync(jt => jt.ID == taskID);
        if (taskEntity == null) { throw new ArgumentException($"Task {taskID} was not found"); }
        var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
            .FirstAsync(td => td.ID == taskEntity.TaskDefinitionID);
        var now = clock.Now();
        await db.TriggeredJobTasks.Update
        (
            taskEntity,
            jt =>
            {
                jt.Status = JobTaskStatus.Values.Running.Value;
                jt.TimeStarted = now;
                jt.TimeInactive = now.Add(taskDefEntity.Timeout);
            }
        );
    }

    public async Task<TriggeredJobDetailModel> TaskCompleted(int jobID, int completedTaskID, bool preserveData, NextTaskModel[] nextTasks)
    {
        var currentTaskEntity = await db.TriggeredJobTasks.Retrieve()
            .FirstOrDefaultAsync(jt => jt.ID == completedTaskID);
        if (currentTaskEntity == null) { throw new ArgumentException($"Task {completedTaskID} was not found"); }
        var now = clock.Now();
        var jobEntity = await JobByID(jobID);
        await db.Transaction
        (
            async () =>
            {
                await new EfTriggeredJobTask(db, currentTaskEntity).End(JobTaskStatus.Values.Completed, preserveData, now);
                await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now);
            }
        );
        var updatedJob = await GetTriggeredJob(jobID);
        return updatedJob;
    }

    private Task<TriggeredJobEntity> JobByID(int jobID) =>
        db.TriggeredJobs.Retrieve().FirstAsync(tj => tj.ID == jobID);

    private async Task AddNextTasks(TriggeredJobEntity jobEntity, TriggeredJobTaskEntity? currentTaskEntity, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
        var currentTaskSequence = currentTaskEntity?.Sequence ?? 0;
        var howMany = nextTasks.Length;
        await ResequenceTasks(jobEntity.ID, currentTaskSequence, howMany);
        var generation = (currentTaskEntity?.Generation ?? 0) + 1;
        var sequence = currentTaskSequence + 1;
        foreach (var nextTask in nextTasks)
        {
            var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
                .FirstOrDefaultAsync(td => td.JobDefinitionID == jobEntity.JobDefinitionID && td.TaskKey == nextTask.TaskKey.Value);
            if (taskDefEntity == null)
            {
                throw new ArgumentException($"Task '{nextTask.TaskKey.DisplayText}' was not found");
            }
            var taskEntity = new TriggeredJobTaskEntity
            {
                TriggeredJobID = jobEntity.ID,
                TaskDefinitionID = taskDefEntity.ID,
                Generation = generation,
                Sequence = sequence,
                TimeAdded = now,
                TimeActive = DateTimeOffset.MinValue,
                TimeInactive = DateTimeOffset.MaxValue,
                TaskData = nextTask.TaskData,
                Status = JobTaskStatus.Values.Pending.Value
            };
            await db.TriggeredJobTasks.Create(taskEntity);
            if (currentTaskEntity != null)
            {
                await db.HierarchicalTriggeredJobTasks.Create
                (
                    new HierarchicalTriggeredJobTaskEntity
                    {
                        ParentTaskID = currentTaskEntity.ID,
                        ChildTaskID = taskEntity.ID
                    }
                );
            }
            sequence++;
        }
    }

    private async Task ResequenceTasks(int jobID, int currentTaskSequence, int howMany)
    {
        var tasksToResequence = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID && t.Sequence > currentTaskSequence)
            .OrderBy(t => t.Sequence)
            .ToArrayAsync();
        foreach (var task in tasksToResequence)
        {
            await db.TriggeredJobTasks.Update
            (
                task,
                t =>
                {
                    t.Sequence += howMany;
                }
            );
        }
    }

    private static TriggeredJobTaskModel CreateTriggeredJobTaskModel(JobTaskDefinitionEntity taskDefEntity, TriggeredJobTaskEntity taskEntity, IEnumerable<LogEntryEntity> entries) =>
        new TriggeredJobTaskModel
        (
            taskEntity.ID,
            new JobTaskDefinitionModel(taskDefEntity.ID, new JobTaskKey(taskDefEntity.DisplayText)),
            JobTaskStatus.Values.Value(taskEntity.Status),
            taskEntity.TaskData,
            entries
                .Select
                (
                    e => new LogEntryModel
                    (
                        e.ID,
                        AppEventSeverity.Values.Value(e.Severity),
                        e.Category,
                        e.Message,
                        e.Details
                    )
                )
                .ToArray()
        );

    public async Task<TriggeredJobDetailModel> TaskFailed(int jobID, int failedTaskID, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, string category, string message, string details)
    {
        var now = clock.Now();
        await db.Transaction
        (
            async () =>
            {
                await db.LogEntries.Create
                (
                    new LogEntryEntity
                    {
                        TaskID = failedTaskID,
                        Severity = AppEventSeverity.Values.CriticalError.Value,
                        Category = category,
                        Message = message,
                        Details = details,
                        TimeOccurred = now
                    }
                );
                var currentTaskEntity = await db.TriggeredJobTasks.Retrieve().FirstAsync(t => t.ID == failedTaskID);
                if (errorStatus.Equals(JobTaskStatus.Values.Retry))
                {
                    await Retry(currentTaskEntity, retryAfter, now);
                }
                else
                {
                    if (errorStatus.Equals(JobTaskStatus.Values.Canceled))
                    {
                        var pendingTaskEntities = await db.TriggeredJobTasks.Retrieve()
                            .Where(t => t.TriggeredJobID == jobID && t.Status == JobTaskStatus.Values.Pending)
                            .ToArrayAsync();
                        foreach (var pendingTaskEntity in pendingTaskEntities)
                        {
                            await new EfTriggeredJobTask(db, pendingTaskEntity).End(errorStatus, true, now);
                        }
                    }
                    await new EfTriggeredJobTask(db, currentTaskEntity).End(errorStatus, true, now);
                }
                var jobEntity = await JobByID(jobID);
                await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now);
            }
        );
        var jobDetail = await GetTriggeredJob(jobID);
        return jobDetail;
    }

    private async Task Retry(TriggeredJobTaskEntity currentTaskEntity, TimeSpan retryAfter, DateTimeOffset now)
    {
        var timeToRetry = now.Add(retryAfter);
        var job = await db.TriggeredJobs.Retrieve()
            .FirstAsync(j => j.ID == currentTaskEntity.TriggeredJobID);
        if (timeToRetry > job.TimeInactive)
        {
            await db.TriggeredJobTasks.Update
            (
                currentTaskEntity,
                t =>
                {
                    t.Status = JobTaskStatus.Values.Failed.Value;
                    t.TimeEnded = now;
                }
            );
            await db.LogEntries.Create
            (
                new LogEntryEntity
                {
                    TaskID = currentTaskEntity.ID,
                    Severity = AppEventSeverity.Values.CriticalError,
                    TimeOccurred = now,
                    Category = JobErrors.JobTimeoutCategory,
                    Message = JobErrors.JobTimeoutMessage,
                    Details = ""
                }
            );
        }
        else
        {
            await ResequenceTasks(currentTaskEntity.TriggeredJobID, currentTaskEntity.Sequence, 1);
            await db.TriggeredJobTasks.Update
            (
                currentTaskEntity,
                t =>
                {
                    t.Status = JobTaskStatus.Values.Completed.Value;
                    t.TimeEnded = now;
                }
            );
            await LogMessage(currentTaskEntity.ID, "Retried", "Retried", "");
            var retryTask = new TriggeredJobTaskEntity
            {
                Status = JobTaskStatus.Values.Retry.Value,
                Generation = currentTaskEntity.Generation,
                Sequence = currentTaskEntity.Sequence + 1,
                TimeAdded = now,
                TimeActive = timeToRetry,
                TaskData = currentTaskEntity.TaskData,
                TaskDefinitionID = currentTaskEntity.TaskDefinitionID,
                TriggeredJobID = currentTaskEntity.TriggeredJobID
            };
            await db.TriggeredJobTasks.Create(retryTask);
            var parentTaskID = await db.HierarchicalTriggeredJobTasks.Retrieve()
                .Where(ht => ht.ChildTaskID == currentTaskEntity.ID)
                .Select(ht => (int?)ht.ParentTaskID)
                .FirstOrDefaultAsync();
            if (parentTaskID.HasValue)
            {
                await db.HierarchicalTriggeredJobTasks.Create
                (
                    new HierarchicalTriggeredJobTaskEntity
                    {
                        ParentTaskID = parentTaskID.Value,
                        ChildTaskID = retryTask.ID
                    }
                );
            }
        }
    }

    public Task LogMessage(int taskID, string category, string message, string details) =>
        db.LogEntries.Create
        (
            new LogEntryEntity
            {
                TaskID = taskID,
                Severity = AppEventSeverity.Values.Information.Value,
                Category = category,
                Message = message,
                Details = details,
                TimeOccurred = clock.Now()
            }
        );
}
