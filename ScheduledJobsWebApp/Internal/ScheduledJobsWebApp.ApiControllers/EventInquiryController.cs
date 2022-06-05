// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class EventInquiryController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public EventInquiryController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Notifications(CancellationToken ct)
    {
        var result = await api.Group("EventInquiry").Action<EmptyRequest, WebViewResult>("Notifications").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<EventSummaryModel[]>> GetRecentNotifications(CancellationToken ct)
    {
        return api.Group("EventInquiry").Action<EmptyRequest, EventSummaryModel[]>("GetRecentNotifications").Execute(new EmptyRequest(), ct);
    }

    public async Task<IActionResult> NotificationDetail(GetNotificationDetailRequest model, CancellationToken ct)
    {
        var result = await api.Group("EventInquiry").Action<GetNotificationDetailRequest, WebViewResult>("NotificationDetail").Execute(model, ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<EventNotificationDetailModel>> GetNotificationDetail([FromBody] GetNotificationDetailRequest model, CancellationToken ct)
    {
        return api.Group("EventInquiry").Action<GetNotificationDetailRequest, EventNotificationDetailModel>("GetNotificationDetail").Execute(model, ct);
    }
}