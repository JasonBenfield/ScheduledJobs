using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfEventNotificationInquiry
{
    private readonly JobDbContext db;

    public EfEventNotificationInquiry(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<EventNotificationModel> Notification(int id)
    {
        var evtWithDefEntity = await Query()
            .Where(evt => evt.Event.ID == id)
            .FirstAsync();
        return CreateEventNotificationModel(evtWithDefEntity);
    }

    public async Task<EventNotificationModel[]> Recent()
    {
        var evtWithDefs = await Query()
            .OrderByDescending(evtWithDef => evtWithDef.Event.TimeAdded)
            .Take(50)
            .ToArrayAsync();
        var evtNotModels = new List<EventNotificationModel>();
        foreach (var evtWithDef in evtWithDefs)
        {
            var evtModel = CreateEventNotificationModel(evtWithDef);
            evtNotModels.Add(evtModel);
        }
        return evtNotModels.ToArray();
    }

    private static EventNotificationModel CreateEventNotificationModel(EventWithDefinitionEntity evtWithDef) =>
        new EventNotificationModel
        (
            evtWithDef.Event.ID,
            new EventDefinitionModel
            (
                evtWithDef.Definition.ID,
                new EventKey(evtWithDef.Definition.DisplayText)
            ),
            evtWithDef.Event.SourceKey,
            evtWithDef.Event.SourceData,
            evtWithDef.Event.TimeAdded,
            evtWithDef.Event.TimeActive,
            evtWithDef.Event.TimeInactive
        );

    private IQueryable<EventWithDefinitionEntity> Query() =>
        db.EventNotifications.Retrieve()
            .Join
            (
                db.EventDefinitions.Retrieve(),
                n => n.EventDefinitionID,
                d => d.ID,
                (n, d) => new EventWithDefinitionEntity { Event = n, Definition = d }
            );

    private sealed class EventWithDefinitionEntity
    {
        public EventNotificationEntity Event { get; set; } = new EventNotificationEntity();
        public EventDefinitionEntity Definition { get; set; } = new EventDefinitionEntity();
    }
}
