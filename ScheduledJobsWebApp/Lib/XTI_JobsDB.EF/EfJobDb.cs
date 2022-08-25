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
            .FirstOrDefaultAsync(td => td.JobDefinitionID == jobDefinitionEntity.ID && td.TaskKey == task.TaskKey.Value);
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
        var evtDefEntity = await db.EventDefinitions.Retrieve()
            .FirstOrDefaultAsync(ed => ed.EventKey == eventKey.Value);
        if (evtDefEntity == null)
        {
            throw new ArgumentException($"Event '{eventKey.DisplayText}' not found");
        }
        var now = clock.Now();
        if (now >= evtDefEntity.TimeToStartNotifications)
        {
            var duplicateHandling = DuplicateHandling.Values.Value(evtDefEntity.DuplicateHandling);
            foreach (var source in sources)
            {
                var duplicateNotifications = await GetDuplicateNotifications(evtDefEntity, duplicateHandling, source);
                var timeActive = now;
                var activeFor = evtDefEntity.ActiveFor;
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
                        EventDefinitionID = evtDefEntity.ID,
                        SourceKey = source.SourceKey,
                        SourceData = source.SourceData,
                        TimeAdded = now,
                        TimeActive = timeActive,
                        TimeInactive = timeInactive,
                        TimeToDelete = now.Add(evtDefEntity.DeleteAfter)
                    };
                    await db.EventNotifications.Create(notificationEntity);
                    eventNotifications.Add
                    (
                        new EventNotificationModel
                        (
                            notificationEntity.ID,
                            new EventDefinitionModel(evtDefEntity.ID, new EventKey(evtDefEntity.DisplayText)),
                            notificationEntity.SourceKey,
                            notificationEntity.SourceData,
                            notificationEntity.TimeAdded,
                            notificationEntity.TimeActive,
                            notificationEntity.TimeInactive
                        )
                    );
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

    public async Task<TriggeredJobWithTasksModel[]> RetryJobs(JobKey jobKey)
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
        var retryJobIDs = retryTasks.Select(rt => rt.TriggeredJobID).Distinct().ToList();
        var pendingJobIDs = db.TriggeredJobTasks.Retrieve()
            .Where(tjt => retryJobIDs.Contains(tjt.TriggeredJobID))
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
        var triggeredJobModels = new List<TriggeredJobWithTasksModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    public async Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime)
    {
        var jobDefEntity = await db.JobDefinitions.Retrieve()
            .FirstOrDefaultAsync(jd => jd.JobKey == jobKey.Value);
        if (jobDefEntity == null)
        {
            throw new ArgumentException($"Job '{jobKey.DisplayText}' was not found");
        }
        var eventDefinitionID = db.EventDefinitions.Retrieve()
            .Where(ed => ed.EventKey == eventKey.Value)
            .Select(ed => ed.ID);
        var triggeredJobEventNotificationIDs = db.TriggeredJobs.Retrieve()
            .Where(tj => tj.JobDefinitionID == jobDefEntity.ID)
            .Select(tj => tj.EventNotificationID);
        var now = clock.Now();
        var notificationEntities = await db.EventNotifications.Retrieve()
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
        foreach (var notificationEntity in notificationEntities)
        {
            var jobEntity = new TriggeredJobEntity
            {
                EventNotificationID = notificationEntity.ID,
                JobDefinitionID = jobDefEntity.ID
            };
            await db.TriggeredJobs.Create(jobEntity);
            var job = new TriggeredJobModel
            (
                jobEntity.ID,
                new JobDefinitionModel
                (
                    jobEntity.ID,
                    new JobKey(jobDefEntity.DisplayText)
                ),
                jobEntity.EventNotificationID
            );
            var pendingJob = new PendingJobModel(job, notificationEntity.SourceKey, notificationEntity.SourceData);
            pendingJobs.Add(pendingJob);
        }
        return pendingJobs.ToArray();
    }

    public async Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID)
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
        var triggeredJobModels = new List<TriggeredJobWithTasksModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    private Task<TriggeredJobWithTasksModel> GetTriggeredJob(int jobID) =>
        new EfTriggeredJobDetail(db, jobID).Value();

    private Task<TriggeredJobWithTasksModel> GetTriggeredJob(TriggeredJobWithDefinitionEntity jobWithDef) =>
        new EfTriggeredJobDetail(db, jobWithDef).Value();

    public async Task<TriggeredJobWithTasksModel> StartJob(int jobID, NextTaskModel[] nextTasks)
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
        var taskEntity = await GetTask(taskID);
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

    public async Task<TriggeredJobWithTasksModel> TaskCompleted(int completedTaskID, bool preserveData, NextTaskModel[] nextTasks)
    {
        var currentTaskEntity = await GetTask(completedTaskID);
        var now = clock.Now();
        var jobEntity = await JobByID(currentTaskEntity.TriggeredJobID);
        await db.Transaction
        (
            async () =>
            {
                await CreateEfTriggeredJobTask(currentTaskEntity).Complete(preserveData);
                await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now);
            }
        );
        var updatedJob = await GetTriggeredJob(currentTaskEntity.TriggeredJobID);
        return updatedJob;
    }

    private Task<TriggeredJobEntity> JobByID(int jobID) =>
        db.TriggeredJobs.Retrieve().FirstAsync(tj => tj.ID == jobID);

    private async Task AddNextTasks(TriggeredJobEntity jobEntity, TriggeredJobTaskEntity? currentTaskEntity, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
        if (currentTaskEntity != null)
        {
            await CreateEfTriggeredJobTask(currentTaskEntity).ResequenceTasks(howMany: nextTasks.Length);
        }
        var generation = (currentTaskEntity?.Generation ?? 0) + 1;
        var currentTaskSequence = currentTaskEntity?.Sequence ?? 0;
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

    public Task JobCancelled(int taskID, string reason) =>
        db.Transaction
        (
            async () =>
            {
                var taskEntity = await GetTask(taskID);
                var efTask = CreateEfTriggeredJobTask(taskEntity);
                await efTask.Cancel();
                await efTask.LogMessage("Cancelled", reason, "");
            }
        );

    public async Task<TriggeredJobWithTasksModel> TaskFailed(int failedTaskID, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, string category, string message, string details)
    {
        TriggeredJobTaskEntity? taskEntity = null;
        await db.Transaction
        (
            async () => taskEntity = await _TaskFailed
            (
                failedTaskID,
                errorStatus,
                retryAfter,
                nextTasks,
                category,
                message,
                details
            )
        );
        var jobDetail = await GetTriggeredJob(taskEntity?.TriggeredJobID ?? 0);
        return jobDetail;
    }

    private async Task<TriggeredJobTaskEntity> _TaskFailed(int failedTaskID, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, string category, string message, string details)
    {
        var now = clock.Now();
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
        var currentTaskEntity = await GetTask(failedTaskID);
        var efTask = CreateEfTriggeredJobTask(currentTaskEntity);
        if (errorStatus.Equals(JobTaskStatus.Values.Retry))
        {
            await Retry(currentTaskEntity, retryAfter, now);
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Canceled))
        {
            await efTask.Cancel();
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Failed))
        {
            await efTask.Fail();
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Completed))
        {
            await efTask.Complete(true);
        }
        else
        {
            throw new ArgumentException($"Error status '{errorStatus.DisplayText}' is not valid");
        }
        var jobEntity = await JobByID(currentTaskEntity.TriggeredJobID);
        await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now);
        return currentTaskEntity;
    }

    private Task<TriggeredJobTaskEntity> GetTask(int taskID) =>
        db.TriggeredJobTasks.Retrieve().FirstAsync(t => t.ID == taskID);

    private EfTriggeredJobTask CreateEfTriggeredJobTask(TriggeredJobTaskEntity taskEntity) =>
        new EfTriggeredJobTask(db, taskEntity, clock);

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
            await CreateEfTriggeredJobTask(currentTaskEntity).Retry(timeToRetry);
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
