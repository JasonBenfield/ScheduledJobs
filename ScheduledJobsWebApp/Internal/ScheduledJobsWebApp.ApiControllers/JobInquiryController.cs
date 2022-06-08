// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class JobInquiryController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public JobInquiryController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> FailedJobs(CancellationToken ct)
    {
        var result = await api.Group("JobInquiry").Action<EmptyRequest, WebViewResult>("FailedJobs").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetFailedJobs(CancellationToken ct)
    {
        return api.Group("JobInquiry").Action<EmptyRequest, JobSummaryModel[]>("GetFailedJobs").Execute(new EmptyRequest(), ct);
    }

    public async Task<IActionResult> RecentJobs(CancellationToken ct)
    {
        var result = await api.Group("JobInquiry").Action<EmptyRequest, WebViewResult>("RecentJobs").Execute(new EmptyRequest(), ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetRecentJobs(CancellationToken ct)
    {
        return api.Group("JobInquiry").Action<EmptyRequest, JobSummaryModel[]>("GetRecentJobs").Execute(new EmptyRequest(), ct);
    }

    public async Task<IActionResult> JobDetail(GetJobDetailRequest model, CancellationToken ct)
    {
        var result = await api.Group("JobInquiry").Action<GetJobDetailRequest, WebViewResult>("JobDetail").Execute(model, ct);
        return View(result.Data.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> GetJobDetail([FromBody] GetJobDetailRequest model, CancellationToken ct)
    {
        return api.Group("JobInquiry").Action<GetJobDetailRequest, TriggeredJobDetailModel>("GetJobDetail").Execute(model, ct);
    }
}