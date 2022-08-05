// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobDefinitionsGroup : AppClientGroup
{
    public JobDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "JobDefinitions")
    {
        Actions = new JobDefinitionsGroupActions(Index: CreateGetAction<EmptyRequest>("Index"), GetJobDefinitions: CreatePostAction<EmptyRequest, JobDefinitionModel[]>("GetJobDefinitions"), GetRecentTriggeredJobs: CreatePostAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>("GetRecentTriggeredJobs"));
    }

    public JobDefinitionsGroupActions Actions { get; }

    public Task<JobDefinitionModel[]> GetJobDefinitions() => Actions.GetJobDefinitions.Post("", new EmptyRequest());
    public Task<JobSummaryModel[]> GetRecentTriggeredJobs(GetRecentTriggeredJobsByDefinitionRequest model) => Actions.GetRecentTriggeredJobs.Post("", model);
    public sealed record JobDefinitionsGroupActions(AppClientGetAction<EmptyRequest> Index, AppClientPostAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions, AppClientPostAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs);
}