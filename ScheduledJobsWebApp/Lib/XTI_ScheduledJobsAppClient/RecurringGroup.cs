// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class RecurringGroup : AppClientGroup
{
    public RecurringGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Recurring")
    {
        Actions = new RecurringGroupActions(TimeoutTasks: CreatePostAction<EmptyRequest, EmptyActionResult>("TimeoutTasks"), PurgeJobsAndEvents: CreatePostAction<EmptyRequest, EmptyActionResult>("PurgeJobsAndEvents"));
    }

    public RecurringGroupActions Actions { get; }

    public Task<EmptyActionResult> TimeoutTasks() => Actions.TimeoutTasks.Post("", new EmptyRequest());
    public Task<EmptyActionResult> PurgeJobsAndEvents() => Actions.PurgeJobsAndEvents.Post("", new EmptyRequest());
    public sealed record RecurringGroupActions(AppClientPostAction<EmptyRequest, EmptyActionResult> TimeoutTasks, AppClientPostAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents);
}