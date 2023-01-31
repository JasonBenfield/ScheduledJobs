namespace XTI_ScheduledJobTests;

internal sealed class OnDemandJobTest
{
    [Test]
    public async Task ShouldTriggerJobOnDemand()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => { },
            jobs => BuildJobs(jobs)
        );
        var triggeredJobs = await host.TriggerJobOnDemand
        (
            DemoJobs.DoSomething.JobKey,
            new DoSomethingData { SourceID = 23 }
        );
        Assert.That
        (
            triggeredJobs.Length,
            Is.EqualTo(1),
            "Should trigger job on demand"
        );
    }

    [Test]
    public async Task ShouldTriggerMultipleJobsOnDemand()
    {
        var host = TestHost.CreateDefault();
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
                SourceID = 23,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 4 }
                }
            },
            new DoSomethingData
            {
                SourceID = 24,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 5 }
                }
            }
        );
        Assert.That(triggeredJobs.Length, Is.EqualTo(2), "Should trigger multiple jobs on demand");
    }

    [Test]
    public async Task ShouldCompleteJobOnDemand()
    {
        var host = TestHost.CreateDefault();
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
                SourceID = 23,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 4 }
                }
            }
        );
        Assert.That
        (
            triggeredJobs[0].Status(),
            Is.EqualTo(JobTaskStatus.Values.Completed),
            "Should complete job on demand"
        );
    }

    [Test]
    public async Task ShouldCompleteAllJobsOnDemand()
    {
        var host = TestHost.CreateDefault();
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
                SourceID = 23,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 4 }
                }
            },
            new DoSomethingData
            {
                SourceID = 24,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 5 }
                }
            }
        );
        Assert.That
        (
            triggeredJobs.Select(j => j.Status()).ToArray(),
            Has.All.EqualTo(JobTaskStatus.Values.Completed),
            "Should complete all jobs on demand"
        );
    }

    [Test]
    public async Task ShouldProcessAllJobsOnDemand()
    {
        var host = TestHost.CreateDefault();
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
                SourceID = 23,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 4 }
                }
            },
            new DoSomethingData
            {
                SourceID = 24,
                Items = new[]
                {
                    new DoSomethingItemData { ItemID = 5 }
                }
            }
        );
        Assert.That
        (
            triggeredJobs.Select(tj => tj.Tasks(DemoJobs.DoSomething.TaskItem01)[0].Data<DoSomethingItemData>().ItemID).ToArray(),
            Is.EqualTo(new[] { 4, 5 }),
            "Should process all jobs on demand"
        );
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            j =>
            {
                foreach (var task in DemoJobs.DoSomething.GetAllTasks())
                {
                    j.AddTask(task);
                }
            }
        )
        .AddJob
        (
            DemoJobs.DoSomethingElse.JobKey,
            j =>
            {
                foreach (var task in DemoJobs.DoSomethingElse.GetAllTasks())
                {
                    j.AddTask(task);
                }
            }
        );

}
