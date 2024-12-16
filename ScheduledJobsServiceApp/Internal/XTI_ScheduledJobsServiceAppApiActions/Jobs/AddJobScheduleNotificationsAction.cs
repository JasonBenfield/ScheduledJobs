using XTI_ScheduledJobsAppClient;

namespace XTI_ScheduledJobsServiceAppApiActions.Jobs;

public sealed class AddJobScheduleNotificationsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly ScheduledJobsAppClient schdJobClient;

    public AddJobScheduleNotificationsAction(ScheduledJobsAppClient schdJobClient)
    {
        this.schdJobClient = schdJobClient;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        await schdJobClient.Recurring.AddJobScheduleNotifications(stoppingToken);
        return new EmptyActionResult();
    }
}
