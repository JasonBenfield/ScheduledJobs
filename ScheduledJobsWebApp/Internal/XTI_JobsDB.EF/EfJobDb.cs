using XTI_Core;
using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfJobDb : IJobDb
{
    private readonly JobDbContext db;

    public EfJobDb(JobDbContext db)
    {
        this.db = db;
    }

    public async Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents)
    {
        foreach (var registeredEvent in registeredEvents)
        {
            var eventDefinition = await db.EventDefinitions.Retrieve()
                .FirstOrDefaultAsync(ed => ed.EventKey == registeredEvent.EventKey.Value);
            if (eventDefinition == null)
            {
                eventDefinition = new EventDefinitionEntity
                {
                    EventKey = registeredEvent.EventKey.Value,
                    CompareSourceKeyAndDataForDuplication = registeredEvent.CompareSourceKeyAndDataForDuplication,
                    DuplicateHandling = registeredEvent.DuplicateHandling,
                    TimeToStartNotifications = registeredEvent.TimeToStartNotifications,
                    ActiveFor = registeredEvent.ActiveFor.ToString()
                };
                await db.EventDefinitions.Create(eventDefinition);
            }
        }
    }

    public async Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs)
    {
        foreach (var registeredJob in registeredJobs)
        {
            var jobDefinitionEntity = await db.JobDefinitions.Retrieve()
                .FirstOrDefaultAsync(jd => jd.JobKey == registeredJob.JobKey.Value);
            if (jobDefinitionEntity == null)
            {
                jobDefinitionEntity = new JobDefinitionEntity
                {
                    JobKey = registeredJob.JobKey.Value
                };
                await db.JobDefinitions.Create(jobDefinitionEntity);
            }
            foreach (var taskKey in registeredJob.TaskKeys)
            {
                var taskDefinitionEntity = await db.JobTaskDefinitions.Retrieve()
                    .FirstOrDefaultAsync(td => td.TaskKey == taskKey.Value);
                if (taskDefinitionEntity == null)
                {
                    taskDefinitionEntity = new JobTaskDefinitionEntity
                    {
                        JobDefinitionID = jobDefinitionEntity.ID,
                        TaskKey = taskKey.Value
                    };
                    await db.JobTaskDefinitions.Create(taskDefinitionEntity);
                }
            }
        }
    }

    public async Task<EventNotificationModel[]> AddNotifications(EventKey eventKey, EventSource[] sources, DateTimeOffset now)
    {
        var eventNotifications = new List<EventNotificationModel>();
        var eventDefinition = await db.EventDefinitions.Retrieve()
            .FirstOrDefaultAsync(ed => ed.EventKey == eventKey.Value);
        if (eventDefinition == null)
        {
            throw new ArgumentException($"Event '{eventKey.DisplayText}' not found");
        }
        if (now >= eventDefinition.TimeToStartNotifications)
        {
            var duplicateHandling = DuplicateHandling.Values.Value(eventDefinition.DuplicateHandling);
            foreach (var source in sources)
            {
                var duplicateNotifications = await GetDuplicateNotifications(eventDefinition, duplicateHandling, source);
                var timeActive = now;
                var activeFor = TimeSpan.Parse(eventDefinition.ActiveFor);
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
                    var notificationEntity = await AddEventNotificationEntity(now, eventDefinition, source, timeActive, timeInactive);
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

    private async Task<EventNotificationEntity> AddEventNotificationEntity(DateTimeOffset now, EventDefinitionEntity eventDefinition, EventSource source, DateTimeOffset timeActive, DateTimeOffset timeInactive)
    {
        var notificationEntity = new EventNotificationEntity
        {
            EventDefinitionID = eventDefinition.ID,
            SourceKey = source.SourceKey,
            SourceData = source.SourceData,
            TimeAdded = now,
            TimeActive = timeActive,
            TimeInactive = timeInactive
        };
        await db.EventNotifications.Create(notificationEntity);
        return notificationEntity;
    }

    public async Task<TriggeredJobDetailModel[]> Retry(JobKey jobKey, DateTimeOffset now)
    {
        var jobDefinitionID = db.JobDefinitions.Retrieve()
            .Where(jd => jd.JobKey == jobKey.Value)
            .Select(jd => jd.ID);
        var triggeredJobIDs = db.TriggeredJobs.Retrieve()
            .Where(tj => jobDefinitionID.Contains(tj.JobDefinitionID))
            .Select(tj => tj.ID);
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

    public async Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset now)
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
        var eventNotifications = await db.EventNotifications.Retrieve()
            .Where
            (
                en =>
                    eventDefinitionID.Contains(en.EventDefinitionID)
                    && !triggeredJobEventNotificationIDs.Contains(en.ID)
                    && now >= en.TimeActive
                    && now < en.TimeInactive
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
            var triggeredJob = CreateTriggeredJobDetailModel(triggeredJobEntity, jobDefinitionEntity, new TriggeredJobTaskModel[0]);
            var pendingJob = new PendingJobModel(triggeredJob.Job, eventNotification.SourceData);
            pendingJobs.Add(pendingJob);
        }
        return pendingJobs.ToArray();
    }

    public async Task<TriggeredJobDetailModel[]> TriggeredJobs(EventNotificationModel notification)
    {
        var triggeredJobs = await
            db.TriggeredJobs.Retrieve()
                .Where(tj => tj.EventNotificationID == notification.ID)
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
        var taskIDs = db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID)
            .Select(t => t.ID);
        var hierarchicalTaskEntities = await db.HierarchicalTriggeredJobTasks.Retrieve()
            .Where(ht => taskIDs.Contains(ht.ParentTaskID))
            .ToArrayAsync();
        var taskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID)
            .Join
            (
                db.JobTaskDefinitions.Retrieve(),
                t => t.TaskDefinitionID,
                td => td.ID,
                (t, td) => new { Task = t, Definition = td }
            )
            .ToArrayAsync();
        var joinedTaskEntities = taskEntities
            .GroupJoin
            (
                hierarchicalTaskEntities,
                t => t.Task.ID,
                ht => ht.ParentTaskID,
                (t, ht) => new { TaskWithDef = t, HierarchicalParentTasks = ht }
            )
            .Select(grouped => new { Task = grouped.TaskWithDef, HierarchicalParentTask = grouped.HierarchicalParentTasks.FirstOrDefault() })
            .GroupJoin
            (
                taskEntities,
                grouped => grouped.HierarchicalParentTask?.ParentTaskID ?? 0,
                t => t.Task.ID,
                (grouped, t) => new { TaskWithDef = grouped.Task, ParentTasks = t }
            )
            .Select(grouped => new { TaskWithDef = grouped.TaskWithDef, ParentTask = grouped.ParentTasks.FirstOrDefault() })
            .OrderBy(grouped => grouped.ParentTask?.Task.Generation ?? 0)
            .ThenBy(grouped => grouped.ParentTask?.Task.Sequence ?? 0)
            .ThenBy(grouped => grouped.TaskWithDef.Task.Sequence)
            .Select(grouped => grouped.TaskWithDef);
        foreach (var t in joinedTaskEntities)
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
                    new JobKey(jobDefEntity.JobKey)
                )
            ),
            tasks
        );

    public async Task<TriggeredJobDetailModel> StartJob(TriggeredJobModel pendingJob, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
        await AddNextTasks(pendingJob, null, nextTasks, now);
        var updatedJob = await GetTriggeredJob(pendingJob.ID);
        return updatedJob;
    }

    public async Task StartTask(TriggeredJobTaskModel pendingTask, DateTimeOffset now)
    {
        var taskEntity = await db.TriggeredJobTasks.Retrieve()
            .FirstOrDefaultAsync(jt => jt.ID == pendingTask.ID);
        if (taskEntity == null) { throw new ArgumentException($"Task {pendingTask.ID} was not found"); }
        await db.TriggeredJobTasks.Update
        (
            taskEntity,
            jt =>
            {
                jt.Status = JobTaskStatus.Values.Running.Value;
                jt.TimeStarted = now;
            }
        );
    }

    public async Task<TriggeredJobDetailModel> TaskCompleted(TriggeredJobModel pendingJob, TriggeredJobTaskModel currentTask, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
        var currentTaskEntity = await db.TriggeredJobTasks.Retrieve()
            .FirstOrDefaultAsync(jt => jt.ID == currentTask.ID);
        if (currentTaskEntity == null) { throw new ArgumentException($"Task {currentTask.ID} was not found"); }
        await db.Transaction
        (
            async () =>
            {
                await EndTask(currentTaskEntity, JobTaskStatus.Values.Completed, now);
                await AddNextTasks(pendingJob, currentTaskEntity, nextTasks, now);
            }
        );
        var updatedJob = await GetTriggeredJob(pendingJob.ID);
        return updatedJob;
    }

    private async Task AddNextTasks(TriggeredJobModel job, TriggeredJobTaskEntity? currentTaskEntity, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
        var generation = (currentTaskEntity?.Generation ?? 0) + 1;
        var maxSequence = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == job.ID && t.Generation == generation)
            .OrderByDescending(t => t.Sequence)
            .Select(t => t.Sequence)
            .FirstOrDefaultAsync();
        var sequence = maxSequence + 1;
        foreach (var nextTask in nextTasks)
        {
            var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
                .FirstOrDefaultAsync(td => td.JobDefinitionID == job.JobDefinition.ID && td.TaskKey == nextTask.TaskKey.Value);
            if (taskDefEntity == null)
            {
                throw new ArgumentException($"Task '{nextTask.TaskKey.DisplayText}' was not found");
            }
            var taskEntity = new TriggeredJobTaskEntity
            {
                TriggeredJobID = job.ID,
                TaskDefinitionID = taskDefEntity.ID,
                Generation = generation,
                Sequence = sequence,
                TimeAdded = now,
                TimeActive = DateTimeOffset.MinValue,
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

    private static TriggeredJobTaskModel CreateTriggeredJobTaskModel(JobTaskDefinitionEntity taskDefEntity, TriggeredJobTaskEntity taskEntity, IEnumerable<LogEntryEntity> entries) =>
        new TriggeredJobTaskModel
        (
            taskEntity.ID,
            new JobTaskDefinitionModel(taskDefEntity.ID, new JobTaskKey(taskDefEntity.TaskKey)),
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

    public async Task<TriggeredJobDetailModel> TaskFailed(TriggeredJobModel job, TriggeredJobTaskModel task, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, string category, string message, string details, DateTimeOffset now)
    {
        await db.Transaction
        (
            async () =>
            {
                var currentTaskEntity = await db.TriggeredJobTasks.Retrieve().FirstAsync(t => t.ID == task.ID);
                if (errorStatus.Equals(JobTaskStatus.Values.Retry))
                {
                    await Retry(currentTaskEntity, retryAfter, now);
                }
                else
                {
                    if (errorStatus.Equals(JobTaskStatus.Values.Canceled))
                    {
                        var pendingTaskEntities = await db.TriggeredJobTasks.Retrieve()
                            .Where(t => t.TriggeredJobID == job.ID && t.Status == JobTaskStatus.Values.Pending)
                            .ToArrayAsync();
                        foreach (var pendingTaskEntity in pendingTaskEntities)
                        {
                            await EndTask(pendingTaskEntity, errorStatus, now);
                        }
                    }
                    await EndTask(currentTaskEntity, errorStatus, now);
                }
                await db.LogEntries.Create
                (
                    new LogEntryEntity
                    {
                        TaskID = task.ID,
                        Severity = AppEventSeverity.Values.CriticalError.Value,
                        Category = category,
                        Message = message,
                        Details = details
                    }
                );
                await AddNextTasks(job, currentTaskEntity, nextTasks, now);
            }
        );
        var jobDetail = await GetTriggeredJob(job.ID);
        return jobDetail;
    }

    private async Task Retry(TriggeredJobTaskEntity currentTaskEntity, TimeSpan retryAfter, DateTimeOffset now)
    {
        await db.TriggeredJobTasks.Update
        (
            currentTaskEntity,
            t =>
            {
                t.Status = JobTaskStatus.Values.Completed.Value;
                t.TimeEnded = now;
            }
        );
        var subsequentTasks = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == currentTaskEntity.TriggeredJobID && t.Generation == currentTaskEntity.Generation && t.Sequence > currentTaskEntity.Sequence)
            .ToArrayAsync();
        foreach (var subsequentTask in subsequentTasks)
        {
            await db.TriggeredJobTasks.Update
            (
                subsequentTask,
                t => t.Sequence = t.Sequence + 1
            );
        }
        await db.TriggeredJobTasks.Create
        (
            new TriggeredJobTaskEntity
            {
                Status = JobTaskStatus.Values.Retry.Value,
                Generation = currentTaskEntity.Generation,
                Sequence = currentTaskEntity.Sequence + 1,
                TimeAdded = now,
                TimeActive = now.Add(retryAfter),
                TaskData = currentTaskEntity.TaskData,
                TaskDefinitionID = currentTaskEntity.TaskDefinitionID,
                TriggeredJobID = currentTaskEntity.TriggeredJobID
            }
        );
    }

    private Task EndTask(TriggeredJobTaskEntity taskEntity, JobTaskStatus status, DateTimeOffset now) =>
        db.TriggeredJobTasks.Update
        (
            taskEntity,
            t =>
            {
                t.Status = status.Value;
                t.TimeEnded = now;
            }
        );

    public Task LogMessage(TriggeredJobTaskModel task, string category, string message, string details, DateTimeOffset now) =>
        db.LogEntries.Create
        (
            new LogEntryEntity
            {
                TaskID = task.ID,
                Severity = AppEventSeverity.Values.Information.Value,
                Category = category,
                Message = message,
                Details = details
            }
        );
}
