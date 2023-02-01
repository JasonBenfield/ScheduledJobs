using System.Text.Json;
using XTI_Core;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class OnDemandJobTest
{
    [Test]
    public async Task ShouldRunOnDemandJob()
    {
        var host = TestHost.CreateDefault(XtiEnvironment.Development);
        await host.Setup();
        await host.Register
        (
            events => { },
            jobs => BuildJobs(jobs)
        );
        var triggeredJobs = await host.TriggerJobOnDemand
        (
            DemoJobs.DoSomething.JobKey,
            new DoSomethingData
            {
                TargetID = 5
            }
        );
        Console.WriteLine(XtiSerializer.Serialize(triggeredJobs.Select(tj => tj.Model),  new JsonSerializerOptions { WriteIndented = true }));
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
