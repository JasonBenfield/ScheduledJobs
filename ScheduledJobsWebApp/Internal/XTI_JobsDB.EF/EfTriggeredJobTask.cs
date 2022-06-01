﻿using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfTriggeredJobTask
{
    private readonly JobDbContext db;
    private readonly TriggeredJobTaskEntity entity;

    public EfTriggeredJobTask(JobDbContext db, TriggeredJobTaskEntity entity)
    {
        this.db = db;
        this.entity = entity;
    }

    public Task End(JobTaskStatus status, DateTimeOffset now) =>
        db.TriggeredJobTasks.Update
        (
            entity,
            t =>
            {
                t.Status = status.Value;
                t.TimeEnded = now;
            }
        );
}