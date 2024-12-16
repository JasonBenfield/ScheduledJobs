using Microsoft.EntityFrameworkCore;
using XTI_Core;

namespace XTI_ScheduledJobsWebAppApiActions.Tasks;

public sealed class EditTaskDataAction : AppAction<EditTaskDataRequest, EmptyActionResult>
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public EditTaskDataAction(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<EmptyActionResult> Execute(EditTaskDataRequest model, CancellationToken stoppingToken)
    {
        await db.Transaction
        (
            async () =>
            {
                var currentTaskEntity = await db.TriggeredJobTasks.Retrieve()
                    .FirstAsync(t => t.ID == model.TaskID);
                var status = JobTaskStatus.Values.Value(currentTaskEntity.Status);
                if (!status.Equals(JobTaskStatus.Values.Failed))
                {
                    throw new AppException(string.Format(TaskErrors.TaskWithStatusCannotEditTaskData, status.DisplayText));
                }
                await new EfTriggeredJobTask(db, currentTaskEntity, clock).EditTaskData(model.TaskData);
            }
        );
        return new EmptyActionResult();
    }
}
