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

    public async Task EditTaskData(string taskData)
    {
        if(entity.TaskData != taskData)
        {
            await LogMessage("OriginalTaskData", entity.TaskData, "");
            await db.TriggeredJobTasks.Update(entity, t => t.TaskData = taskData);
        }
    }

    public async Task Timeout()
    {
        await Fail();
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
            }
        );
    }

    public async Task Cancel()
    {
        var pendingTaskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == entity.TriggeredJobID && t.Status == JobTaskStatus.Values.Pending)
            .ToArrayAsync();
        foreach (var pendingTaskEntity in pendingTaskEntities)
        {
            await new EfTriggeredJobTask(db, pendingTaskEntity, clock).End(JobTaskStatus.Values.Canceled, true);
        }
        await End(JobTaskStatus.Values.Canceled, true);
    }

    public Task Fail() => End(JobTaskStatus.Values.Failed, true);

    public Task Complete(bool preserveData) => End(JobTaskStatus.Values.Completed, preserveData);

    public async Task Retry(DateTimeOffset timeToRetry)
    {
        await ResequenceTasks(1);
        await db.TriggeredJobTasks.Update
        (
            entity,
            t =>
            {
                t.Status = JobTaskStatus.Values.Completed.Value;
                t.TimeEnded = clock.Now();
            }
        );
        await LogMessage("Retried", "Retried", "");
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
        await db.TriggeredJobTasks.Create(retryTask);
        var parentTaskID = await db.HierarchicalTriggeredJobTasks.Retrieve()
            .Where(ht => ht.ChildTaskID == entity.ID)
            .Select(ht => (int?)ht.ParentTaskID)
            .FirstOrDefaultAsync();
        if (parentTaskID.HasValue)
        {
            await db.HierarchicalTriggeredJobTasks.Create
            (
                new HierarchicalTriggeredJobTaskEntity
                {
                    ParentTaskID = parentTaskID.Value,
                    ChildTaskID = retryTask.ID
                }
            );
        }
    }

    public Task End(JobTaskStatus status, bool preserveData) =>
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
            }
        );

    public Task LogMessage(string category, string message, string details) =>
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
            }
        );

    public async Task ResequenceTasks(int howMany)
    {
        var tasksToResequence = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == entity.TriggeredJobID && t.Sequence > entity.Sequence)
            .OrderBy(t => t.Sequence)
            .ToArrayAsync();
        foreach (var task in tasksToResequence)
        {
            await db.TriggeredJobTasks.Update
            (
                task,
                t =>
                {
                    t.Sequence += howMany;
                }
            );
        }
    }
}
