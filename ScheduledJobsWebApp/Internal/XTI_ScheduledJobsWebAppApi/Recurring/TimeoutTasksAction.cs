using Microsoft.EntityFrameworkCore;
using XTI_Core;
using XTI_Jobs;

namespace XTI_ScheduledJobsWebAppApi.Recurring;

internal sealed class TimeoutTasksAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public TimeoutTasksAction(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model)
    {
        var now = clock.Now();
        var runningTasks = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.Status == JobTaskStatus.Values.Running.Value && t.TimeInactive < now)
            .ToArrayAsync();
        foreach(var runningTask in runningTasks)
        {
            await new EfTriggeredJobTask(db, runningTask).End(JobTaskStatus.Values.Failed, clock.Now());
            await db.LogEntries.Create
            (
                new LogEntryEntity
                {
                    TaskID = runningTask.ID,
                    Severity = AppEventSeverity.Values.CriticalError.Value,
                    Category = JobErrors.TaskTimeoutCategory,
                    Message = JobErrors.TaskTimeoutMessage,
                    Details = "",
                    TimeOccurred = now
                }
            );
        }
        return new EmptyActionResult();
    }
}