// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class JobDefinitionsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public JobDefinitionsController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Group("JobDefinitions").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<JobDefinitionModel[]>> GetJobDefinitions(CancellationToken ct)
    {
        return api.Group("JobDefinitions").Action<EmptyRequest, JobDefinitionModel[]>("GetJobDefinitions").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetRecentTriggeredJobs([FromBody] GetRecentTriggeredJobsByDefinitionRequest model, CancellationToken ct)
    {
        return api.Group("JobDefinitions").Action<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>("GetRecentTriggeredJobs").Execute(model, ct);
    }
}