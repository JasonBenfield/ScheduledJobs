using XTI_ScheduledJobsWebAppApi.Events;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private EventsGroup? _Events;

    public EventsGroup Events { get => _Events ?? throw new ArgumentNullException(nameof(_Events)); }

    partial void createEventsGroup(IServiceProvider sp)
    {
        _Events = new EventsGroup
        (
            source.AddGroup(nameof(Events)),
            sp
        );
    }
}