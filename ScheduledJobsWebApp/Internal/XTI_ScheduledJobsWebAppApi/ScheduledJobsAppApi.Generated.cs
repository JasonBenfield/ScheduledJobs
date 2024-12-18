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
public sealed partial class ScheduledJobsAppApi : WebAppApiWrapper
{
    internal ScheduledJobsAppApi(AppApi source, ScheduledJobsAppApiBuilder builder) : base(source)
    {
        EventDefinitions = builder.EventDefinitions.Build();
        EventInquiry = builder.EventInquiry.Build();
        Events = builder.Events.Build();
        Home = builder.Home.Build();
        JobDefinitions = builder.JobDefinitions.Build();
        JobInquiry = builder.JobInquiry.Build();
        Jobs = builder.Jobs.Build();
        Recurring = builder.Recurring.Build();
        Tasks = builder.Tasks.Build();
        Configure();
    }

    partial void Configure();
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