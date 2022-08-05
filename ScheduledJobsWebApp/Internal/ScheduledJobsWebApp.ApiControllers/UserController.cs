// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class UserController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public UserController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<ResourcePathAccess[]>> GetUserAccess([FromBody] ResourcePath[] model, CancellationToken ct)
    {
        return api.Group("User").Action<ResourcePath[], ResourcePathAccess[]>("GetUserAccess").Execute(model, ct);
    }

    public async Task<IActionResult> AccessDenied(CancellationToken ct)
    {
        var result = await api.Group("User").Action<EmptyRequest, WebViewResult>("AccessDenied").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    public async Task<IActionResult> Error(CancellationToken ct)
    {
        var result = await api.Group("User").Action<EmptyRequest, WebViewResult>("Error").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    public async Task<IActionResult> Logout(LogoutRequest model, CancellationToken ct)
    {
        var result = await api.Group("User").Action<LogoutRequest, WebRedirectResult>("Logout").Execute(model, ct);
        return Redirect(result.Data.Url);
    }
}