using XTI_ScheduledJobsWebAppApi.JobInquiry;

namespace XTI_ScheduledJobsWebAppApi;

internal static class JobInquiryGroupExtensions
{
    public static void AddJobInquiryGroupServices(this IServiceCollection services)
    {
        services.AddScoped<FailedJobsView>();
        services.AddScoped<GetFailedJobsAction>();
        services.AddScoped<JobDetailView>();
        services.AddScoped<GetJobDetailAction>();
    }
}