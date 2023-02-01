using System.Text.Json;
using XTI_Core;
using XTI_JobsDB.EF;
using XTI_Schedule;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class ScheduleJobTest
{
    [Test]
    public async Task ShouldScheduleJob()
    {
        var host = TestHost.CreateDefault(XtiEnvironment.Development);
        await host.Setup();
        await host.Register
        (
            events => { },
            jobs => BuildJobs(jobs)
        );
        await host.RegisterJobSchedule
        (
            DemoJobs.DoSomething.JobKey,
            Schedule.EveryDay().At(TimeRange.From(new TimeOnly(8)).ForOneHour())
        );
        await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            job => job
                .TimeoutAfter(TimeSpan.FromHours(1))
                .AddTask(DemoJobs.DoSomething.Task01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.Task02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskFinal).TimeoutAfter(TimeSpan.FromMinutes(5))
        );
}
