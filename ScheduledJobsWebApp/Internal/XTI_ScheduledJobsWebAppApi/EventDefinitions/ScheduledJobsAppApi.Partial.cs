using XTI_ScheduledJobsWebAppApi.EventDefinitions;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private EventDefinitionsGroup? _EventDefinitions;

    public EventDefinitionsGroup EventDefinitions { get => _EventDefinitions ?? throw new ArgumentNullException(nameof(_EventDefinitions)); }

    partial void createEventDefinitionsGroup(IServiceProvider sp)
    {
        _EventDefinitions = new EventDefinitionsGroup
        (
            source.AddGroup(nameof(EventDefinitions)),
            sp
        );
    }
}