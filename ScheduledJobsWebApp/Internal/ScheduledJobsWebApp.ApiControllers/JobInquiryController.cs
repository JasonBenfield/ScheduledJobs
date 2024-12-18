// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class JobInquiryController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public JobInquiryController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    public async Task<IActionResult> FailedJobs(CancellationToken ct)
    {
        var result = await api.JobInquiry.FailedJobs.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetFailedJobs(CancellationToken ct)
    {
        return api.JobInquiry.GetFailedJobs.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> GetJobDetail([FromBody] GetJobDetailRequest requestData, CancellationToken ct)
    {
        return api.JobInquiry.GetJobDetail.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<JobSummaryModel[]>> GetRecentJobs(CancellationToken ct)
    {
        return api.JobInquiry.GetRecentJobs.Execute(new EmptyRequest(), ct);
    }

    public async Task<IActionResult> JobDetail(GetJobDetailRequest requestData, CancellationToken ct)
    {
        var result = await api.JobInquiry.JobDetail.Execute(requestData, ct);
        return View(result.Data!.ViewName);
    }

    public async Task<IActionResult> RecentJobs(CancellationToken ct)
    {
        var result = await api.JobInquiry.RecentJobs.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }
}