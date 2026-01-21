using XTI_Core;
using XTI_Jobs.Abstractions;
using XTI_Schedule;

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

    public async Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents, CancellationToken ct)
    {
        foreach (var registeredEvent in registeredEvents)
        {
            await AddOrUpdateRegisteredEvent(registeredEvent, ct);
        }
    }

    private async Task<EventDefinitionEntity> AddOrUpdateRegisteredEvent(RegisteredEvent registeredEvent, CancellationToken ct)
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
            await db.EventDefinitions.Create(evtDefEntity, ct);
        }
        return evtDefEntity;
    }

    public Task AddOrUpdateJobSchedules(JobKey jobKey, AggregateSchedule aggregateSchedule, TimeSpan deleteAfter, CancellationToken ct) =>
        db.Transaction(() => _AddOrUpdateJobSchedules(jobKey, aggregateSchedule, deleteAfter, ct));

    private async Task _AddOrUpdateJobSchedules(JobKey jobKey, AggregateSchedule aggregateSchedule, TimeSpan deleteAfter, CancellationToken ct)
    {
        var jobDefEntity = await GetJobDefinition(jobKey, ct);
        if (jobDefEntity == null)
        {
            throw new Exception($"Job '{jobKey.DisplayText} not found");
        }
        var evtDefEntity = await AddOrUpdateRegisteredEvent
        (
            new RegisteredEvent
            (
                EventKey.Scheduled(jobKey),
                false,
                DuplicateHandling.Values.Ignore,
                DateTimeOffset.MinValue,
                TimeSpan.Zero,
                deleteAfter
            ),
            ct
        );
        await AddOrUpdateJobSchedule(jobDefEntity, aggregateSchedule, ct);
        var minTime = clock.Now().Add(TimeSpan.FromMinutes(5));
        var dateTimeRanges = aggregateSchedule.DateTimeRanges(DateRange.From(clock.Now().Date).ForOneDay())
            .Where(dtr => dtr.Start >= minTime)
            .ToArray();
        var futureEventNotifications = await db.EventNotifications.Retrieve()
            .Where(en => en.EventDefinitionID == evtDefEntity.ID && en.TimeActive >= minTime)
            .ToArrayAsync(ct);
        var eventNotificationsToDelete = futureEventNotifications
            .Where(en => !dateTimeRanges.Any(dtr => en.TimeActive == dtr.Start && en.TimeInactive == dtr.End))
            .ToArray();
        await db.EventNotifications.DeleteRange(eventNotificationsToDelete);
        await new EfEventNotificationCreator(db, clock).AddJobScheduleNotifications(evtDefEntity, dateTimeRanges, ct);
    }

    private async Task AddOrUpdateJobSchedule(JobDefinitionEntity jobDefEntity, AggregateSchedule aggregateSchedule, CancellationToken ct)
    {
        var serialized = aggregateSchedule.Serialize();
        var schdEntity = await db.JobSchedules.Retrieve()
            .FirstOrDefaultAsync(s => s.JobDefinitionID == jobDefEntity.ID, ct);
        if (schdEntity == null)
        {
            await db.JobSchedules.Create
            (
                new JobScheduleEntity
                {
                    JobDefinitionID = jobDefEntity.ID,
                    Serialized = serialized
                },
                ct
            );
        }
        else
        {
            await db.JobSchedules.Update
            (
                schdEntity,
                s => s.Serialized = serialized,
                ct
            );
        }
    }

    public async Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs, CancellationToken ct)
    {
        foreach (var registeredJob in registeredJobs)
        {
            var jobDefEntity = await AddOrUpdateJobDefinition(registeredJob, ct);
            foreach (var task in registeredJob.Tasks)
            {
                await AddOrUpdateTaskDefinition(jobDefEntity, task, ct);
            }
        }
    }

    private async Task<JobDefinitionEntity> AddOrUpdateJobDefinition(RegisteredJob registeredJob, CancellationToken ct)
    {
        var jobDefEntity = await GetJobDefinition(registeredJob.JobKey, ct);
        if (jobDefEntity == null)
        {
            jobDefEntity = new JobDefinitionEntity
            {
                JobKey = registeredJob.JobKey.Value,
                DisplayText = registeredJob.JobKey.DisplayText,
                Timeout = registeredJob.Timeout,
                DeleteAfter = registeredJob.DeleteAfter
            };
            await db.JobDefinitions.Create(jobDefEntity, ct);
        }
        else
        {
            await db.JobDefinitions.Update
            (
                jobDefEntity,
                jd => jd.Timeout = registeredJob.Timeout,
                ct
            );
        }
        return jobDefEntity;
    }

    private Task<JobDefinitionEntity?> GetJobDefinition(JobKey jobKey, CancellationToken ct) =>
        db.JobDefinitions.Retrieve()
            .FirstOrDefaultAsync(jd => jd.JobKey == jobKey.Value, ct);

    private async Task AddOrUpdateTaskDefinition(JobDefinitionEntity jobDefinitionEntity, RegisteredJobTask task, CancellationToken ct)
    {
        var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
            .FirstOrDefaultAsync(td => td.JobDefinitionID == jobDefinitionEntity.ID && td.TaskKey == task.TaskKey.Value, ct);
        if (taskDefEntity == null)
        {
            taskDefEntity = new JobTaskDefinitionEntity
            {
                JobDefinitionID = jobDefinitionEntity.ID,
                TaskKey = task.TaskKey.Value,
                DisplayText = task.TaskKey.DisplayText,
                Timeout = task.Timeout
            };
            await db.JobTaskDefinitions.Create(taskDefEntity, ct);
        }
        else
        {
            await db.JobTaskDefinitions.Update
            (
                taskDefEntity,
                td => td.Timeout = task.Timeout,
                ct
            );
        }
    }

    public async Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, XtiEventSource[] sources, CancellationToken ct)
    {
        var evtDefEntity = await db.EventDefinitions.Retrieve()
            .FirstOrDefaultAsync(ed => ed.EventKey == eventKey.Value, ct);
        if (evtDefEntity == null)
        {
            throw new ArgumentException($"Event '{eventKey.DisplayText}' not found");
        }
        var timeActive = clock.Now();
        var activeFor = evtDefEntity.ActiveFor;
        var creator = new EfEventNotificationCreator(db, clock);
        var eventNotifications = await creator.AddEventNotifications(sources, evtDefEntity, timeActive, activeFor, ct);
        return eventNotifications;
    }

    public async Task DeleteJobsWithNoTasks(EventKey eventKey, JobKey jobKey, CancellationToken ct)
    {
        var now = clock.Now();
        var eventDefinitionID = db.EventDefinitions.Retrieve()
            .Where(ed => ed.EventKey == eventKey.Value)
            .Select(ed => ed.ID);
        var eventNotificationIDs = db.EventNotifications.Retrieve()
            .Where(en => eventDefinitionID.Contains(en.EventDefinitionID) && en.TimeActive <= now)
            .Select(en => en.ID);
        var jobDefinitionID = db.JobDefinitions.Retrieve()
            .Where(jd => jd.JobKey == jobKey.Value)
            .Select(jd => jd.ID);
        var jobIDsForTasks = db.TriggeredJobTasks.Retrieve()
            .Select(tjt => tjt.TriggeredJobID);
        var jobs = await db.TriggeredJobs.Retrieve()
            .Where
            (
                tj =>
                    eventNotificationIDs.Contains(tj.EventNotificationID) &&
                    jobDefinitionID.Contains(tj.JobDefinitionID) &&
                    !jobIDsForTasks.Contains(tj.ID)
            )
            .ToArrayAsync(ct);
        foreach (var job in jobs)
        {
            await db.TriggeredJobs.Delete(job, ct);
        }
    }

    public async Task<TriggeredJobWithTasksModel[]> RetryJobs(EventKey eventKey, JobKey jobKey, CancellationToken ct)
    {
        var eventDefinitionID = db.EventDefinitions.Retrieve()
            .Where(ed => ed.EventKey == eventKey.Value)
            .Select(ed => ed.ID);
        var eventNotificationIDs = db.EventNotifications.Retrieve()
            .Where(en => eventDefinitionID.Contains(en.EventDefinitionID))
            .Select(en => en.ID);
        var jobDefinitionID = db.JobDefinitions.Retrieve()
            .Where(jd => jd.JobKey == jobKey.Value)
            .Select(jd => jd.ID);
        var triggeredJobIDs = db.TriggeredJobs.Retrieve()
            .Where
            (
                tj =>
                    eventNotificationIDs.Contains(tj.EventNotificationID) &&
                    jobDefinitionID.Contains(tj.JobDefinitionID)
            )
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
            .ToArrayAsync(ct);
        foreach (var retryTask in retryTasks)
        {
            await db.TriggeredJobTasks.Update
            (
                retryTask,
                t => t.Status = JobTaskStatus.Values.Pending.Value,
                ct
            );
        }
        var skipTasks = await db.TriggeredJobTasks.Retrieve()
            .Where
            (
                t =>
                    triggeredJobIDs.Contains(t.TriggeredJobID) &&
                    t.Status == JobTaskStatus.Values.Skip.Value &&
                    now >= t.TimeActive
            )
            .ToArrayAsync(ct);
        foreach (var skipTask in skipTasks)
        {
            await db.TriggeredJobTasks.Update
            (
                skipTask,
                t => t.Status = JobTaskStatus.Values.Completed.Value,
                ct
            );
        }
        var retryJobIDs = retryTasks.Select(rt => rt.TriggeredJobID).Distinct().ToList();
        var skipJobIDs = skipTasks.Select(rt => rt.TriggeredJobID).Distinct().ToList();
        var pendingJobIDs = db.TriggeredJobTasks.Retrieve()
            .Where(tjt => retryJobIDs.Contains(tjt.TriggeredJobID) || skipJobIDs.Contains(tjt.TriggeredJobID))
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
                .ToArrayAsync(ct);
        var triggeredJobModels = new List<TriggeredJobWithTasksModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob, ct);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    public async Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime, CancellationToken ct)
    {
        var jobDefEntity = await db.JobDefinitions.Retrieve()
            .FirstOrDefaultAsync(jd => jd.JobKey == jobKey.Value, ct);
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
            .ToArrayAsync(ct);
        var pendingJobs = new List<PendingJobModel>();
        foreach (var notificationEntity in notificationEntities)
        {
            var jobEntity = new TriggeredJobEntity
            {
                EventNotificationID = notificationEntity.ID,
                JobDefinitionID = jobDefEntity.ID
            };
            await db.TriggeredJobs.Create(jobEntity, ct);
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

    public async Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID, CancellationToken ct)
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
                .ToArrayAsync(ct);
        var triggeredJobModels = new List<TriggeredJobWithTasksModel>();
        foreach (var triggeredJob in triggeredJobs)
        {
            var jobModel = await GetTriggeredJob(triggeredJob, ct);
            triggeredJobModels.Add(jobModel);
        }
        return triggeredJobModels.ToArray();
    }

    private Task<TriggeredJobWithTasksModel> GetTriggeredJob(int jobID, CancellationToken ct) =>
        new EfTriggeredJobDetail(db, jobID).Value(ct);

    private Task<TriggeredJobWithTasksModel> GetTriggeredJob(TriggeredJobWithDefinitionEntity jobWithDef, CancellationToken ct) =>
        new EfTriggeredJobDetail(db, jobWithDef).Value(ct);

    public async Task<TriggeredJobWithTasksModel> StartJob(int jobID, NextTaskModel[] nextTasks, CancellationToken ct)
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
                    .FirstAsync(ct);
                var now = clock.Now();
                await db.TriggeredJobs.Update
                (
                    jobWithDefEntity.Job,
                    j =>
                    {
                        j.TimeInactive = now.Add(jobWithDefEntity.Definition.Timeout);
                        j.TimeToDelete = now.Add(jobWithDefEntity.Definition.DeleteAfter);
                    },
                    ct
                );
                await AddNextTasks(jobWithDefEntity.Job, null, nextTasks, now, ct);
            }
        );
        var updatedJob = await GetTriggeredJob(jobID, ct);
        return updatedJob;
    }

    public async Task StartTask(int taskID, CancellationToken ct)
    {
        var taskEntity = await GetTask(taskID, ct);
        var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
            .FirstAsync(td => td.ID == taskEntity.TaskDefinitionID, ct);
        var now = clock.Now();
        await db.TriggeredJobTasks.Update
        (
            taskEntity,
            jt =>
            {
                jt.Status = JobTaskStatus.Values.Running.Value;
                jt.TimeStarted = now;
                jt.TimeInactive = now.Add(taskDefEntity.Timeout);
            },
            ct
        );
    }

    public async Task<TriggeredJobWithTasksModel> TaskCompleted(int completedTaskID, bool preserveData, NextTaskModel[] nextTasks, CancellationToken ct)
    {
        var currentTaskEntity = await GetTask(completedTaskID, ct);
        var now = clock.Now();
        var jobEntity = await JobByID(currentTaskEntity.TriggeredJobID, ct);
        await db.Transaction
        (
            async () =>
            {
                await CreateEfTriggeredJobTask(currentTaskEntity).Complete(preserveData, ct);
                await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now, ct);
            }
        );
        var updatedJob = await GetTriggeredJob(currentTaskEntity.TriggeredJobID, ct);
        return updatedJob;
    }

    private Task<TriggeredJobEntity> JobByID(int jobID, CancellationToken ct) =>
        db.TriggeredJobs.Retrieve().FirstAsync(tj => tj.ID == jobID, ct);

    private async Task AddNextTasks(TriggeredJobEntity jobEntity, TriggeredJobTaskEntity? currentTaskEntity, NextTaskModel[] nextTasks, DateTimeOffset now, CancellationToken ct)
    {
        if (currentTaskEntity != null)
        {
            await CreateEfTriggeredJobTask(currentTaskEntity).ResequenceTasks(howMany: nextTasks.Length, ct);
        }
        var generation = (currentTaskEntity?.Generation ?? 0) + 1;
        var currentTaskSequence = currentTaskEntity?.Sequence ?? 0;
        var sequence = currentTaskSequence + 1;
        foreach (var nextTask in nextTasks)
        {
            var taskDefEntity = await db.JobTaskDefinitions.Retrieve()
                .FirstOrDefaultAsync(td => td.JobDefinitionID == jobEntity.JobDefinitionID && td.TaskKey == nextTask.TaskKey.Value, ct);
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
            await db.TriggeredJobTasks.Create(taskEntity, ct);
            if (currentTaskEntity != null)
            {
                await db.HierarchicalTriggeredJobTasks.Create
                (
                    new HierarchicalTriggeredJobTaskEntity
                    {
                        ParentTaskID = currentTaskEntity.ID,
                        ChildTaskID = taskEntity.ID
                    },
                    ct
                );
            }
            sequence++;
        }
    }

    public Task JobCancelled(int taskID, string reason, CancellationToken ct) =>
        db.Transaction
        (
            async () =>
            {
                var taskEntity = await GetTask(taskID, ct);
                var efTask = CreateEfTriggeredJobTask(taskEntity);
                await efTask.Cancel(ct);
                await efTask.LogMessage("Cancelled", reason, "", ct);
            }
        );

    public async Task<TriggeredJobWithTasksModel> TaskFailed
    (
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        string category,
        string message,
        string details,
        string sourceLogEntryKey,
        CancellationToken ct
    )
    {
        var taskEntity = await db.Transaction
        (
            () => _TaskFailed
            (
                failedTaskID,
                errorStatus,
                retryAfter,
                nextTasks,
                category,
                message,
                details,
                sourceLogEntryKey,
                ct
            )
        );
        var jobDetail = await GetTriggeredJob(taskEntity?.TriggeredJobID ?? 0, ct);
        return jobDetail;
    }

    private async Task<TriggeredJobTaskEntity> _TaskFailed
    (
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        string category,
        string message,
        string details,
        string sourceLogEntryKey,
        CancellationToken ct
    )
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
                TimeOccurred = now,
                SourceLogEntryKey = sourceLogEntryKey
            },
            ct
        );
        var currentTaskEntity = await GetTask(failedTaskID, ct);
        var efTask = CreateEfTriggeredJobTask(currentTaskEntity);
        if (errorStatus.Equals(JobTaskStatus.Values.Retry))
        {
            await Retry(currentTaskEntity, retryAfter, now, ct);
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Canceled))
        {
            await efTask.Cancel(ct);
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Failed))
        {
            await efTask.Fail(ct);
        }
        else if (errorStatus.Equals(JobTaskStatus.Values.Completed))
        {
            await efTask.Complete(true, ct);
        }
        else
        {
            throw new ArgumentException($"Error status '{errorStatus.DisplayText}' is not valid");
        }
        var jobEntity = await JobByID(currentTaskEntity.TriggeredJobID, ct);
        await AddNextTasks(jobEntity, currentTaskEntity, nextTasks, now, ct);
        return currentTaskEntity;
    }

    private Task<TriggeredJobTaskEntity> GetTask(int taskID, CancellationToken ct) =>
        db.TriggeredJobTasks.Retrieve().FirstAsync(t => t.ID == taskID, ct);

    private EfTriggeredJobTask CreateEfTriggeredJobTask(TriggeredJobTaskEntity taskEntity) =>
        new EfTriggeredJobTask(db, taskEntity, clock);

    private async Task Retry(TriggeredJobTaskEntity currentTaskEntity, TimeSpan retryAfter, DateTimeOffset now, CancellationToken ct)
    {
        var timeToRetry = now.Add(retryAfter);
        var job = await db.TriggeredJobs.Retrieve()
            .FirstAsync(j => j.ID == currentTaskEntity.TriggeredJobID, ct);
        if (timeToRetry > job.TimeInactive)
        {
            await db.TriggeredJobTasks.Update
            (
                currentTaskEntity,
                t =>
                {
                    t.Status = JobTaskStatus.Values.Failed.Value;
                    t.TimeEnded = now;
                },
                ct
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
                },
                ct
            );
        }
        else
        {
            await CreateEfTriggeredJobTask(currentTaskEntity).Retry(timeToRetry, ct);
        }
    }

    public Task LogMessage(int taskID, string category, string message, string details, CancellationToken ct) =>
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
            },
            ct
        );

}
