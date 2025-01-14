// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class ScheduledJobsAppClient : AppClient
{
    public ScheduledJobsAppClient(IHttpClientFactory httpClientFactory, XtiTokenAccessorFactory xtiTokenAccessorFactory, AppClientUrl clientUrl, IAppClientSessionKey sessionKey, IAppClientRequestKey requestKey, ScheduledJobsAppClientVersion version) : base(httpClientFactory, xtiTokenAccessorFactory, clientUrl, sessionKey, requestKey, "ScheduledJobs", version.Value)
    {
        EventDefinitions = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new EventDefinitionsGroup(_clientFactory, _tokenAccessor, _url, _options));
        EventInquiry = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new EventInquiryGroup(_clientFactory, _tokenAccessor, _url, _options));
        Events = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new EventsGroup(_clientFactory, _tokenAccessor, _url, _options));
        Home = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new HomeGroup(_clientFactory, _tokenAccessor, _url, _options));
        JobDefinitions = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new JobDefinitionsGroup(_clientFactory, _tokenAccessor, _url, _options));
        JobInquiry = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new JobInquiryGroup(_clientFactory, _tokenAccessor, _url, _options));
        Jobs = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new JobsGroup(_clientFactory, _tokenAccessor, _url, _options));
        Recurring = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new RecurringGroup(_clientFactory, _tokenAccessor, _url, _options));
        Tasks = CreateGroup((_clientFactory, _tokenAccessor, _url, _options) => new TasksGroup(_clientFactory, _tokenAccessor, _url, _options));
        Configure();
    }

    partial void Configure();
    public ScheduledJobsRoleNames RoleNames { get; } = ScheduledJobsRoleNames.Instance;
    public string AppName { get; } = "ScheduledJobs";
    public EventDefinitionsGroup EventDefinitions { get; }
    public EventInquiryGroup EventInquiry { get; }
    public EventsGroup Events { get; }
    public HomeGroup Home { get; }
    public JobDefinitionsGroup JobDefinitions { get; }
    public JobInquiryGroup JobInquiry { get; }
    public JobsGroup Jobs { get; }
    public RecurringGroup Recurring { get; }
    public TasksGroup Tasks { get; }
}