using Microsoft.EntityFrameworkCore;
using XTI_Core;

namespace XTI_ScheduledJobsWebAppApi.Recurring;

internal sealed class PurgeJobsAndEventsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public PurgeJobsAndEventsAction(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        var now = clock.Now();
        var jobIDs = db.TriggeredJobs.Retrieve()
            .Where(job => now >= job.TimeToDelete)
            .Select(job => job.ID);
        var taskIDs = db.TriggeredJobTasks.Retrieve()
            .Where(task => jobIDs.Contains(task.TriggeredJobID))
            .Select(task => task.ID);
        var logEntryEntities = await db.LogEntries.Retrieve()
            .Where(le => taskIDs.Contains(le.TaskID))
            .ToArrayAsync();
        foreach (var logEntryEntity in logEntryEntities)
        {
            await db.LogEntries.Delete(logEntryEntity);
        }
        var hierTaskEntities = await db.HierarchicalTriggeredJobTasks.Retrieve()
            .Where(ht => taskIDs.Contains(ht.ParentTaskID))
            .ToArrayAsync();
        foreach (var hierTaskEntity in hierTaskEntities)
        {
            await db.HierarchicalTriggeredJobTasks.Delete(hierTaskEntity);
        }
        var taskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => taskIDs.Contains(t.ID))
            .ToArrayAsync();
        foreach (var taskEntity in taskEntities)
        {
            await db.TriggeredJobTasks.Delete(taskEntity);
        }
        var jobEntities = await db.TriggeredJobs.Retrieve()
            .Where(j => jobIDs.Contains(j.ID))
            .ToArrayAsync();
        foreach (var jobEntity in jobEntities)
        {
            await db.TriggeredJobs.Delete(jobEntity);
        }
        var excludedEvtNotIDs = db.TriggeredJobs.Retrieve()
            .Select(job => job.EventNotificationID);
        var evtNotEntities = await db.EventNotifications.Retrieve()
            .Where(evtNot => now > evtNot.TimeToDelete && !excludedEvtNotIDs.Contains(evtNot.ID))
            .ToArrayAsync();
        foreach (var evtNotEntity in evtNotEntities)
        {
            await db.EventNotifications.Delete(evtNotEntity);
        }
        return new EmptyActionResult();
    }
}
