// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobDefinitionsGroup : AppClientGroup
{
    public JobDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "JobDefinitions")
    {
        Actions = new JobDefinitionsGroupActions(GetJobDefinitions: CreatePostAction<EmptyRequest, JobDefinitionModel[]>("GetJobDefinitions"), GetRecentTriggeredJobs: CreatePostAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>("GetRecentTriggeredJobs"), Index: CreateGetAction<EmptyRequest>("Index"));
    }

    public JobDefinitionsGroupActions Actions { get; }

    public Task<JobDefinitionModel[]> GetJobDefinitions(CancellationToken ct = default) => Actions.GetJobDefinitions.Post("", new EmptyRequest(), ct);
    public Task<JobSummaryModel[]> GetRecentTriggeredJobs(GetRecentTriggeredJobsByDefinitionRequest requestData, CancellationToken ct = default) => Actions.GetRecentTriggeredJobs.Post("", requestData, ct);
    public sealed record JobDefinitionsGroupActions(AppClientPostAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions, AppClientPostAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs, AppClientGetAction<EmptyRequest> Index);
}