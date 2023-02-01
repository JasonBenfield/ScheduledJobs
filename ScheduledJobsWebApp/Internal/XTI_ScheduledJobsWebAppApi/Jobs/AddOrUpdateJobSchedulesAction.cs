using XTI_Schedule;

namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class AddOrUpdateJobSchedulesAction : AppAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public AddOrUpdateJobSchedulesAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(AddOrUpdateJobSchedulesRequest model, CancellationToken ct)
    {
        var jobKey = new JobKey(model.JobKey);
        var aggregateSchedule = AggregateSchedule.Deserialize(model.Schedules);
        await db.AddOrUpdateJobSchedules(jobKey, aggregateSchedule, model.DeleteAfter);
        return new EmptyActionResult();
    }
}