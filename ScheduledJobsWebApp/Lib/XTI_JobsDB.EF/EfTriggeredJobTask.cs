using XTI_Core;
using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfTriggeredJobTask
{
    private readonly JobDbContext db;
    private readonly TriggeredJobTaskEntity entity;
    private readonly IClock clock;

    public EfTriggeredJobTask(JobDbContext db, TriggeredJobTaskEntity entity, IClock clock)
    {
        this.db = db;
        this.entity = entity;
        this.clock = clock;
    }

    public async Task EditTaskData(string taskData, CancellationToken ct)
    {
        if (entity.TaskData != taskData)
        {
            await LogMessage("OriginalTaskData", entity.TaskData, "", ct);
            await db.TriggeredJobTasks.Update(entity, t => t.TaskData = taskData, ct);
        }
    }

    public async Task Timeout(CancellationToken ct)
    {
        await Fail(ct);
        await db.LogEntries.Create
        (
            new LogEntryEntity
            {
                TaskID = entity.ID,
                Severity = AppEventSeverity.Values.CriticalError.Value,
                Category = JobErrors.TaskTimeoutCategory,
                Message = JobErrors.TaskTimeoutMessage,
                Details = "",
                TimeOccurred = clock.Now()
            },
            ct
        );
    }

    public async Task Cancel(CancellationToken ct)
    {
        var pendingTaskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == entity.TriggeredJobID && t.Status == JobTaskStatus.Values.Pending)
            .ToArrayAsync(ct);
        foreach (var pendingTaskEntity in pendingTaskEntities)
        {
            await new EfTriggeredJobTask(db, pendingTaskEntity, clock).End(JobTaskStatus.Values.Canceled, true, ct);
        }
        await End(JobTaskStatus.Values.Canceled, true, ct);
    }

    public Task Fail(CancellationToken ct) => End(JobTaskStatus.Values.Failed, true, ct);

    public Task Complete(bool preserveData, CancellationToken ct) => End(JobTaskStatus.Values.Completed, preserveData, ct);

    public async Task Skip(CancellationToken ct)
    {
        await End(JobTaskStatus.Values.Skip, true, ct);
        await LogMessage(JobErrors.TaskSkippedCategory, JobErrors.TaskSkippedMessage, "", ct);
    }

    public async Task Retry(DateTimeOffset timeToRetry, CancellationToken ct)
    {
        await ResequenceTasks(1, ct);
        await db.TriggeredJobTasks.Update
        (
            entity,
            t =>
            {
                t.Status = JobTaskStatus.Values.Completed.Value;
                t.TimeEnded = clock.Now();
            },
            ct
        );
        await LogMessage(JobErrors.TaskRetriedCategory, JobErrors.TaskRetriedMessage, "", ct);
        var retryTask = new TriggeredJobTaskEntity
        {
            Status = JobTaskStatus.Values.Retry.Value,
            Generation = entity.Generation,
            Sequence = entity.Sequence + 1,
            TimeAdded = clock.Now(),
            TimeActive = timeToRetry,
            TaskData = entity.TaskData,
            TaskDefinitionID = entity.TaskDefinitionID,
            TriggeredJobID = entity.TriggeredJobID
        };
        await db.TriggeredJobTasks.Create(retryTask, ct);
        var parentTaskID = await db.HierarchicalTriggeredJobTasks.Retrieve()
            .Where(ht => ht.ChildTaskID == entity.ID)
            .Select(ht => (int?)ht.ParentTaskID)
            .FirstOrDefaultAsync(ct);
        if (parentTaskID.HasValue)
        {
            await db.HierarchicalTriggeredJobTasks.Create
            (
                new HierarchicalTriggeredJobTaskEntity
                {
                    ParentTaskID = parentTaskID.Value,
                    ChildTaskID = retryTask.ID
                },
                ct
            );
        }
    }

    public Task End(JobTaskStatus status, bool preserveData, CancellationToken ct) =>
        db.TriggeredJobTasks.Update
        (
            entity,
            t =>
            {
                t.Status = status.Value;
                if (!preserveData)
                {
                    t.TaskData = "";
                }
                if (t.TimeStarted == DateTimeOffset.MaxValue)
                {
                    t.TimeStarted = clock.Now();
                }
                t.TimeEnded = clock.Now();
            },
            ct
        );

    public Task LogMessage(string category, string message, string details, CancellationToken ct) =>
        db.LogEntries.Create
        (
            new LogEntryEntity
            {
                TaskID = entity.ID,
                Severity = AppEventSeverity.Values.Information.Value,
                Category = category,
                Message = message,
                Details = details,
                TimeOccurred = clock.Now()
            },
            ct
        );

    public async Task ResequenceTasks(int howMany, CancellationToken ct)
    {
        var tasksToResequence = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == entity.TriggeredJobID && t.Sequence > entity.Sequence)
            .OrderBy(t => t.Sequence)
            .ToArrayAsync(ct);
        foreach (var task in tasksToResequence)
        {
            await db.TriggeredJobTasks.Update
            (
                task,
                t =>
                {
                    t.Sequence += howMany;
                },
                ct
            );
        }
    }

}
