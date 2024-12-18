using XTI_ScheduledJobsWebAppApiActions.JobInquiry;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class JobInquiryGroupExtensions
{
    internal static void AddJobInquiryServices(this IServiceCollection services)
    {
        services.AddScoped<FailedJobsPage>();
        services.AddScoped<GetFailedJobsAction>();
        services.AddScoped<GetJobDetailAction>();
        services.AddScoped<GetRecentJobsAction>();
        services.AddScoped<JobDetailPage>();
        services.AddScoped<RecentJobsPage>();
    }
}