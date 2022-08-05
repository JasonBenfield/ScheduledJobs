﻿namespace XTI_ScheduledJobsServiceAppApi.Jobs;

public sealed class PurgeJobsAndEventsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly XTI_ScheduledJobsAppClient.ScheduledJobsAppClient schdJobsClient;

    public PurgeJobsAndEventsAction(XTI_ScheduledJobsAppClient.ScheduledJobsAppClient schdJobsClient)
    {
        this.schdJobsClient = schdJobsClient;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        await schdJobsClient.Recurring.PurgeJobsAndEvents();
        return new EmptyActionResult();
    }
}