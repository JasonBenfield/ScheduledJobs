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
}