// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class EventInquiryController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public EventInquiryController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EventNotificationDetailModel>> GetNotificationDetail([FromBody] GetNotificationDetailRequest requestData, CancellationToken ct)
    {
        return api.EventInquiry.GetNotificationDetail.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EventSummaryModel[]>> GetRecentNotifications(CancellationToken ct)
    {
        return api.EventInquiry.GetRecentNotifications.Execute(new EmptyRequest(), ct);
    }

    public async Task<IActionResult> NotificationDetail(GetNotificationDetailRequest requestData, CancellationToken ct)
    {
        var result = await api.EventInquiry.NotificationDetail.Execute(requestData, ct);
        return View(result.Data!.ViewName);
    }

    public async Task<IActionResult> Notifications(CancellationToken ct)
    {
        var result = await api.EventInquiry.Notifications.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }
}