using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfStoredEvents : IStoredEvents
{
    private readonly JobDbContext db;

    public EfStoredEvents(JobDbContext db)
    {
        this.db = db;
    }

    public async Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents)
    {
        foreach (var registeredEvent in registeredEvents)
        {
            var eventDefinition = new EventDefinitionEntity
            {
                EventKey = registeredEvent.EventKey.Value,
                CompareSourceKeyAndDataForDuplication = registeredEvent.CompareSourceKeyAndDataForDuplication,
                DuplicateHandling = registeredEvent.DuplicateHandling
            };
            await db.EventDefinitions.Create(eventDefinition);
        }
    }

    public async Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs)
    {
        foreach (var registeredJob in registeredJobs)
        {
            var jobDefinition = new JobDefinitionEntity
            {
                JobKey = registeredJob.JobKey.Value
            };
            await db.JobDefinitions.Create(jobDefinition);
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
        var duplicateHandling = DuplicateHandling.Values.Value(eventDefinition.DuplicateHandling);
        foreach (var source in sources)
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
            var timeActive = now;
            var timeInactive = DateTimeOffset.MaxValue;
            if (duplicateHandling.Equals(DuplicateHandling.Values.KeepOldest) && duplicateNotifications.Any())
            {
                timeActive = DateTimeOffset.MaxValue;
                timeInactive = now;
            }
            if (duplicateHandling.Equals(DuplicateHandling.Values.KeepNewest))
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
            if (!duplicateHandling.Equals(DuplicateHandling.Values.Ignore) || !duplicateNotifications.Any())
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
                eventNotifications.Add(new EventNotificationModel(notificationEntity.ID));
            }
        }
        return eventNotifications.ToArray();
    }

    public async Task<TriggeredJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset now)
    {
        var jobDefinitionEntity = await db.JobDefinitions.Retrieve().FirstOrDefaultAsync(jd => jd.JobKey == jobKey.Value);
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
        var triggeredJobs = new List<TriggeredJobModel>();
        foreach (var eventNotifiction in eventNotifications)
        {
            var triggeredJobEntity = new TriggeredJobEntity
            {
                EventNotificationID = eventNotifiction.ID,
                JobDefinitionID = jobDefinitionEntity.ID,
                Status = JobStatus.Values.NotRun
            };
            await db.TriggeredJobs.Create(triggeredJobEntity);
            triggeredJobs.Add(CreateTriggeredJobModel(triggeredJobEntity));
        }
        return triggeredJobs.ToArray();
    }

    public async Task<TriggeredJobModel[]> TriggeredJobs(EventNotificationModel notification)
    {
        var triggeredJobs = await db.TriggeredJobs.Retrieve()
            .Where(tj => tj.EventNotificationID == notification.ID)
            .ToArrayAsync();
        return triggeredJobs.Select(tj => CreateTriggeredJobModel(tj)).ToArray();
    }

    private static TriggeredJobModel CreateTriggeredJobModel(TriggeredJobEntity entity) =>
        new TriggeredJobModel(entity.ID, JobStatus.Values.Value(entity.Status));

}
