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
        var triggeredJobModels = new List<TriggeredJobDetailModel>();
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
        var tasks = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID)
            .Join
            (
                db.JobTaskDefinitions.Retrieve(),
                t => t.TaskDefinitionID,
                td => td.ID,
                (t, td) => new { Task = t, Definition = td }
            )
            .ToArrayAsync();
        foreach (var t in tasks)
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
        await AddNextTasks(pendingJob, nextTasks, now);
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
        await db.TriggeredJobTasks.Update
        (
            currentTaskEntity,
            jt =>
            {
                jt.Status = JobTaskStatus.Values.Completed.Value;
                jt.TimeEnded = now;
            }
        );
        await AddNextTasks(pendingJob, nextTasks, now);
        var updatedJob = await GetTriggeredJob(pendingJob.ID);
        return updatedJob;
    }

    private async Task AddNextTasks(TriggeredJobModel job, NextTaskModel[] nextTasks, DateTimeOffset now)
    {
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
                TimeAdded = now,
                TaskData = nextTask.TaskData,
                Status = JobTaskStatus.Values.Pending.Value
            };
            await db.TriggeredJobTasks.Create(taskEntity);
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

    public Task JobCompleted(PendingJobModel job)
    {
        return Task.CompletedTask;
    }

    public async Task<TriggeredJobDetailModel> TaskFailed(TriggeredJobModel job, TriggeredJobTaskModel task, AppEventSeverity severity, string category, string message, string details)
    {
        await db.LogEntries.Create
        (
            new LogEntryEntity
            {
                TaskID = task.ID,
                Severity = severity.Value,
                Category = category,
                Message = message,
                Details = details
            }
        );
        var jobDetail = await GetTriggeredJob(job.ID);
        return jobDetail;
    }
}
