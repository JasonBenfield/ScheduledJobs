using XTI_ScheduledJobsWebAppApiActions.EventInquiry;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class EventInquiryGroupExtensions
{
    internal static void AddEventInquiryServices(this IServiceCollection services)
    {
        services.AddScoped<GetNotificationDetailAction>();
        services.AddScoped<GetRecentNotificationsAction>();
        services.AddScoped<NotificationDetailPage>();
        services.AddScoped<NotificationsPage>();
    }
}