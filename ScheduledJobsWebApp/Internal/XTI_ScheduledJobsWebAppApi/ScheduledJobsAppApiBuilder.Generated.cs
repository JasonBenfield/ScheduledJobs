using XTI_ScheduledJobsWebAppApi.EventDefinitions;
using XTI_ScheduledJobsWebAppApi.EventInquiry;
using XTI_ScheduledJobsWebAppApi.Events;
using XTI_ScheduledJobsWebAppApi.Home;
using XTI_ScheduledJobsWebAppApi.JobDefinitions;
using XTI_ScheduledJobsWebAppApi.JobInquiry;
using XTI_ScheduledJobsWebAppApi.Jobs;
using XTI_ScheduledJobsWebAppApi.Recurring;
using XTI_ScheduledJobsWebAppApi.Tasks;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi;
public sealed partial class ScheduledJobsAppApiBuilder
{
    private readonly AppApi source;
    private readonly IServiceProvider sp;
    public ScheduledJobsAppApiBuilder(IServiceProvider sp, IAppApiUser user)
    {
        source = new AppApi(sp, ScheduledJobsAppKey.Value, user);
        this.sp = sp;
        EventDefinitions = new EventDefinitionsGroupBuilder(source.AddGroup("EventDefinitions"));
        EventInquiry = new EventInquiryGroupBuilder(source.AddGroup("EventInquiry"));
        Events = new EventsGroupBuilder(source.AddGroup("Events"));
        Home = new HomeGroupBuilder(source.AddGroup("Home"));
        JobDefinitions = new JobDefinitionsGroupBuilder(source.AddGroup("JobDefinitions"));
        JobInquiry = new JobInquiryGroupBuilder(source.AddGroup("JobInquiry"));
        Jobs = new JobsGroupBuilder(source.AddGroup("Jobs"));
        Recurring = new RecurringGroupBuilder(source.AddGroup("Recurring"));
        Tasks = new TasksGroupBuilder(source.AddGroup("Tasks"));
        Configure();
    }

    partial void Configure();
    public EventDefinitionsGroupBuilder EventDefinitions { get; }
    public EventInquiryGroupBuilder EventInquiry { get; }
    public EventsGroupBuilder Events { get; }
    public HomeGroupBuilder Home { get; }
    public JobDefinitionsGroupBuilder JobDefinitions { get; }
    public JobInquiryGroupBuilder JobInquiry { get; }
    public JobsGroupBuilder Jobs { get; }
    public RecurringGroupBuilder Recurring { get; }
    public TasksGroupBuilder Tasks { get; }

    public ScheduledJobsAppApi Build() => new ScheduledJobsAppApi(source, this);
}