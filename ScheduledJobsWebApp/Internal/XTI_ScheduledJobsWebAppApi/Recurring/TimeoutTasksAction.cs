using Microsoft.EntityFrameworkCore;
using XTI_Core;

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

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken ct)
    {
        var now = clock.Now();
        var runningTasks = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.Status == JobTaskStatus.Values.Running.Value && t.TimeInactive < now)
            .ToArrayAsync();
        foreach(var runningTask in runningTasks)
        {
            await db.Transaction
            (
                async () =>
                {
                    await new EfTriggeredJobTask(db, runningTask, clock).Fail();
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
            );
        }
        return new EmptyActionResult();
    }
}