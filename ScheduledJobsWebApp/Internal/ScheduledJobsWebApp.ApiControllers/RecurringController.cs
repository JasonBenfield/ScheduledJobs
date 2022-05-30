// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class RecurringController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public RecurringController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> TimeoutTasks()
    {
        return api.Group("Recurring").Action<EmptyRequest, EmptyActionResult>("TimeoutTasks").Execute(new EmptyRequest());
    }
}