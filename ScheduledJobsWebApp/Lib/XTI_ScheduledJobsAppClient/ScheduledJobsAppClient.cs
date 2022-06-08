// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class ScheduledJobsAppClient : AppClient
{
    public ScheduledJobsAppClient(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, ScheduledJobsAppClientVersion version) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "ScheduledJobs", version.Value)
    {
        User = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserGroup(_clientFactory, _tokenAccessor, _url));
        UserCache = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserCacheGroup(_clientFactory, _tokenAccessor, _url));
        Home = GetGroup((_clientFactory, _tokenAccessor, _url) => new HomeGroup(_clientFactory, _tokenAccessor, _url));
        Recurring = GetGroup((_clientFactory, _tokenAccessor, _url) => new RecurringGroup(_clientFactory, _tokenAccessor, _url));
        EventDefinitions = GetGroup((_clientFactory, _tokenAccessor, _url) => new EventDefinitionsGroup(_clientFactory, _tokenAccessor, _url));
        EventInquiry = GetGroup((_clientFactory, _tokenAccessor, _url) => new EventInquiryGroup(_clientFactory, _tokenAccessor, _url));
        Events = GetGroup((_clientFactory, _tokenAccessor, _url) => new EventsGroup(_clientFactory, _tokenAccessor, _url));
        JobDefinitions = GetGroup((_clientFactory, _tokenAccessor, _url) => new JobDefinitionsGroup(_clientFactory, _tokenAccessor, _url));
        JobInquiry = GetGroup((_clientFactory, _tokenAccessor, _url) => new JobInquiryGroup(_clientFactory, _tokenAccessor, _url));
        Jobs = GetGroup((_clientFactory, _tokenAccessor, _url) => new JobsGroup(_clientFactory, _tokenAccessor, _url));
        Tasks = GetGroup((_clientFactory, _tokenAccessor, _url) => new TasksGroup(_clientFactory, _tokenAccessor, _url));
    }

    public ScheduledJobsRoleNames RoleNames { get; } = ScheduledJobsRoleNames.Instance;
    public string AppName { get; } = "ScheduledJobs";
    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public HomeGroup Home { get; }

    public RecurringGroup Recurring { get; }

    public EventDefinitionsGroup EventDefinitions { get; }

    public EventInquiryGroup EventInquiry { get; }

    public EventsGroup Events { get; }

    public JobDefinitionsGroup JobDefinitions { get; }

    public JobInquiryGroup JobInquiry { get; }

    public JobsGroup Jobs { get; }

    public TasksGroup Tasks { get; }
}