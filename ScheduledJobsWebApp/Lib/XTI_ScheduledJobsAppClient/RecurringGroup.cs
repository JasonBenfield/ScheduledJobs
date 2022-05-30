// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class RecurringGroup : AppClientGroup
{
    public RecurringGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Recurring")
    {
    }

    public Task<EmptyActionResult> TimeoutTasks() => Post<EmptyActionResult, EmptyRequest>("TimeoutTasks", "", new EmptyRequest());
}