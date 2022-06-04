namespace XTI_ScheduledJobsServiceAppApi.Jobs;

internal sealed class TimeoutJobsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly XTI_ScheduledJobsAppClient.ScheduledJobsAppClient schdJobsClient;

    public TimeoutJobsAction(XTI_ScheduledJobsAppClient.ScheduledJobsAppClient schdJobsClient)
    {
        this.schdJobsClient = schdJobsClient;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        await schdJobsClient.Recurring.TimeoutTasks();
        return new EmptyActionResult();
    }
}
