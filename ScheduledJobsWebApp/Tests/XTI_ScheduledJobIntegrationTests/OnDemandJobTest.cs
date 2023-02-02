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
        await host.RegisterJobs
        (
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

    private static JobRegistrationBuilder1 BuildJobs(JobRegistrationBuilder jobs) =>
        jobs
            .AddJob(DemoJobs.DoSomething.JobKey)
            .TimeoutAfter(TimeSpan.FromHours(1))
            .AddTasks
            (
                DemoJobs.DoSomething.GetAllTasks(),
                (t, j) => j.TimeoutAfter(TimeSpan.FromMinutes(5))
            );
}
