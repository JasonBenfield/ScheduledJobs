﻿namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class StartJobAction : AppAction<StartJobRequest, TriggeredJobWithTasksModel>
{
    private readonly IJobDb db;

    public StartJobAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel> Execute(StartJobRequest model, CancellationToken ct) =>
        db.StartJob(model.JobID, model.NextTasks);
}
