using XTI_Core;
using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfEventNotificationCreator
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public EfEventNotificationCreator(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<EventNotificationModel[]> AddJobScheduleNotifications(EventDefinitionEntity evtDefEntity, DateTimeRange[] dateTimeRanges, CancellationToken ct)
    {
        var eventNotifications = new List<EventNotificationModel>();
        foreach (var dateTimeRange in dateTimeRanges)
        {
            var timeRangeNotifications = await AddEventNotifications
            (
                [new XtiEventSource($"{dateTimeRange.Format()}", "")],
                evtDefEntity,
                dateTimeRange.Start,
                dateTimeRange.End - dateTimeRange.Start,
                ct
            );
            eventNotifications.AddRange(timeRangeNotifications);
        }
        return eventNotifications.ToArray();
    }

    public async Task<EventNotificationModel[]> AddEventNotifications(XtiEventSource[] sources, EventDefinitionEntity evtDefEntity, DateTimeOffset timeActive, TimeSpan activeFor, CancellationToken ct)
    {
        var eventNotifications = new List<EventNotificationModel>();
        var now = clock.Now();
        if (now >= evtDefEntity.TimeToStartNotifications)
        {
            var duplicateHandling = DuplicateHandling.Values.Value(evtDefEntity.DuplicateHandling);
            foreach (var source in sources)
            {
                var duplicateNotifications = await GetDuplicateNotifications(evtDefEntity, duplicateHandling, source, ct);
                var timeInactive = activeFor == TimeSpan.MaxValue
                    ? DateTimeOffset.MaxValue
                    : timeActive.Add(activeFor);
                if (duplicateHandling.Equals(DuplicateHandling.Values.KeepOldest) && duplicateNotifications.Any())
                {
                    timeActive = DateTimeOffset.MaxValue;
                    timeInactive = now;
                }
                if (duplicateHandling.Equals(DuplicateHandling.Values.KeepNewest))
                {
                    await DeactiveDuplicateEventNotifications(now, duplicateNotifications, ct);
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
                    await db.EventNotifications.Create(notificationEntity, ct);
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

    private async Task<EventNotificationEntity[]> GetDuplicateNotifications(EventDefinitionEntity eventDefinition, DuplicateHandling duplicateHandling, XtiEventSource source, CancellationToken ct)
    {
        EventNotificationEntity[] duplicateNotifications;
        if (duplicateHandling.Equals(DuplicateHandling.Values.KeepAll))
        {
            duplicateNotifications = [];
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
            duplicateNotifications = await duplicateNotificationsQuery.ToArrayAsync(ct);
        }
        return duplicateNotifications;
    }


    private async Task DeactiveDuplicateEventNotifications(DateTimeOffset now, EventNotificationEntity[] duplicateNotifications, CancellationToken ct)
    {
        foreach (var duplicateNotification in duplicateNotifications)
        {
            await db.EventNotifications.Update
            (
                duplicateNotification,
                dn => dn.TimeInactive = now.AddMinutes(-1),
                ct
            );
        }
    }

}
