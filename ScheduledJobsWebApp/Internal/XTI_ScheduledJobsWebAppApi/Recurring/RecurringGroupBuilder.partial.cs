namespace XTI_ScheduledJobsWebAppApi.Recurring;

partial class RecurringGroupBuilder
{
    partial void Configure()
    {
        AddJobScheduleNotifications
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
        TimeoutTasks
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
    }
}
