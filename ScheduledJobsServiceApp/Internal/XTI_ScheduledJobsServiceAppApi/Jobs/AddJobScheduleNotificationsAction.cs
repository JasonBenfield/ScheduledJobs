using XTI_ScheduledJobsAppClient;

namespace XTI_ScheduledJobsServiceAppApi.Jobs;

internal sealed class AddJobScheduleNotificationsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly ScheduledJobsAppClient schdJobClient;

    public AddJobScheduleNotificationsAction(ScheduledJobsAppClient schdJobClient)
    {
        this.schdJobClient = schdJobClient;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        await schdJobClient.Events.AddJobScheduleNotifications(stoppingToken);
        return new EmptyActionResult();
    }
}
