namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class DeleteJobsWithNoTasksRequest
{
    public EventKey EventKey { get; set; } = new EventKey("");
    public JobKey JobKey { get; set; } = new JobKey("");
}

internal sealed class DeleteJobsWithNoTasksAction : AppAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public DeleteJobsWithNoTasksAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(DeleteJobsWithNoTasksRequest model, CancellationToken stoppingToken)
    {
        await db.DeleteJobsWithNoTasks(model.EventKey, model.JobKey);
        return new EmptyActionResult();
    }
}
