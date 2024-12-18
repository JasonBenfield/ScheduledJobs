namespace XTI_ScheduledJobsWebAppApi.Jobs;

partial class JobsGroupBuilder
{
    partial void Configure()
    {
        AddOrUpdateJobSchedules
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
        TriggerJobs
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
        DeleteJobsWithNoTasks
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
        RetryJobs
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes();
        StartJob
            .ThrottleRequestLogging().ForOneHour();
        StartTask
            .ThrottleRequestLogging().ForOneHour();
        TaskCompleted
            .ThrottleRequestLogging().ForOneHour();
        TaskFailed
            .ThrottleRequestLogging().ForOneHour();
    }
}
