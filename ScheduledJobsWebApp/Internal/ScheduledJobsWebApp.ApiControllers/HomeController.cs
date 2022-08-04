// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class HomeController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public HomeController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Group("Home").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }
}