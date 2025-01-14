// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class RecurringGroup : AppClientGroup
{
    public RecurringGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Recurring")
    {
        Actions = new RecurringGroupActions(AddJobScheduleNotifications: CreatePostAction<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications"), PurgeJobsAndEvents: CreatePostAction<EmptyRequest, EmptyActionResult>("PurgeJobsAndEvents"), TimeoutTasks: CreatePostAction<EmptyRequest, EmptyActionResult>("TimeoutTasks"));
        Configure();
    }

    partial void Configure();
    public RecurringGroupActions Actions { get; }

    public Task<EmptyActionResult> AddJobScheduleNotifications(CancellationToken ct = default) => Actions.AddJobScheduleNotifications.Post("", new EmptyRequest(), ct);
    public Task<EmptyActionResult> PurgeJobsAndEvents(CancellationToken ct = default) => Actions.PurgeJobsAndEvents.Post("", new EmptyRequest(), ct);
    public Task<EmptyActionResult> TimeoutTasks(CancellationToken ct = default) => Actions.TimeoutTasks.Post("", new EmptyRequest(), ct);
    public sealed record RecurringGroupActions(AppClientPostAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications, AppClientPostAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents, AppClientPostAction<EmptyRequest, EmptyActionResult> TimeoutTasks);
}