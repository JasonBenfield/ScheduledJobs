namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public sealed class DeleteJobsWithNoTasksAction : AppAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public DeleteJobsWithNoTasksAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(DeleteJobsWithNoTasksRequest deleteRequest, CancellationToken stoppingToken)
    {
        await db.DeleteJobsWithNoTasks(new EventKey(deleteRequest.EventKey), new JobKey(deleteRequest.JobKey));
        return new EmptyActionResult();
    }
}
