// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class UserCacheController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public UserCacheController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> ClearCache([FromBody] string model, CancellationToken ct)
    {
        return api.Group("UserCache").Action<string, EmptyActionResult>("ClearCache").Execute(model, ct);
    }
}