using XTI_ScheduledJobsWebAppApi.EventInquiry;

namespace XTI_ScheduledJobsWebAppApi;

internal static class EventInquiryGroupExtensions
{
    public static void AddEventInquiryGroupServices(this IServiceCollection services)
    {
        services.AddScoped<NotificationsView>();
        services.AddScoped<GetRecentNotificationsAction>();
        services.AddScoped<NotificationDetailView>();
        services.AddScoped<GetNotificationDetailAction>();
    }
}