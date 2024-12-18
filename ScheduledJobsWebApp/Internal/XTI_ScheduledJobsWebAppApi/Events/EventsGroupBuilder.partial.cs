namespace XTI_ScheduledJobsWebAppApi.Events;

partial class EventsGroupBuilder
{
    partial void Configure()
    {
        AddNotifications
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
    }
}
