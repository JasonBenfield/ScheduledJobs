// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobDefinitionsGroup : AppClientGroup
{
    public JobDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "JobDefinitions")
    {
        Actions = new JobDefinitionsActions(clientUrl);
    }

    public JobDefinitionsActions Actions { get; }

    public Task<JobDefinitionModel[]> GetJobDefinitions() => Post<JobDefinitionModel[], EmptyRequest>("GetJobDefinitions", "", new EmptyRequest());
    public Task<JobSummaryModel[]> GetRecentTriggeredJobs(GetRecentTriggeredJobsByDefinitionRequest model) => Post<JobSummaryModel[], GetRecentTriggeredJobsByDefinitionRequest>("GetRecentTriggeredJobs", "", model);
}