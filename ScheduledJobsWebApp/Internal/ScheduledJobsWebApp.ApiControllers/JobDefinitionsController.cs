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

    [HttpPost]
    public Task<ResultContainer<JobDefinitionModel[]>> GetJobDefinitions(CancellationToken ct)
    {
        return api.JobDefinitions.GetJobDefinitions.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetRecentTriggeredJobs([FromBody] GetRecentTriggeredJobsByDefinitionRequest requestData, CancellationToken ct)
    {
        return api.JobDefinitions.GetRecentTriggeredJobs.Execute(requestData, ct);
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.JobDefinitions.Index.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }
}