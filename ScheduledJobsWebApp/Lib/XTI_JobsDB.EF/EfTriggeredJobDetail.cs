using XTI_Core;
using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class EfTriggeredJobDetail
{
    private readonly JobDbContext db;
    private TriggeredJobWithDefinitionEntity? jobWithDef;
    private readonly int jobID;

    internal EfTriggeredJobDetail(JobDbContext db, TriggeredJobWithDefinitionEntity jobWithDef)
        :this(db, jobWithDef.Job.ID)
    {
        this.jobWithDef = jobWithDef;
    }

    public EfTriggeredJobDetail(JobDbContext db, int jobID)
    {
        this.db = db;
        this.jobID = jobID;
    }

    public async Task<TriggeredJobWithTasksModel> Value()
    {
        if(jobWithDef == null)
        {
            jobWithDef = await
                db.TriggeredJobs.Retrieve()
                    .Where(tj => tj.ID == jobID)
                    .Join
                    (
                        db.JobDefinitions.Retrieve(),
                        tj => tj.JobDefinitionID,
                        jd => jd.ID,
                        (tj, jd) => new TriggeredJobWithDefinitionEntity(tj, jd)
                    )
                    .FirstAsync();
        }
        var jobModel = await GetTriggeredJob(jobWithDef);
        return jobModel;
    }

    private async Task<TriggeredJobWithTasksModel> GetTriggeredJob(TriggeredJobWithDefinitionEntity jobWithDef)
    {
        var tasks = await TaskModels(jobWithDef.Job.ID);
        var jobModel = CreateTriggeredJobDetailModel(jobWithDef.Job, jobWithDef.Definition, tasks);
        return jobModel;
    }

    private async Task<TriggeredJobTaskModel[]> TaskModels(int jobID)
    {
        var taskModels = new List<TriggeredJobTaskModel>();
        var taskEntities = await db.TriggeredJobTasks.Retrieve()
            .Where(t => t.TriggeredJobID == jobID)
            .Join
            (
                db.JobTaskDefinitions.Retrieve(),
                t => t.TaskDefinitionID,
                td => td.ID,
                (t, td) => new { Task = t, Definition = td }
            )
            .OrderBy(grouped => grouped.Task.Sequence)
            .ToArrayAsync();
        foreach (var t in taskEntities)
        {
            var entries = await db.LogEntries.Retrieve()
                .Where(e => e.TaskID == t.Task.ID)
                .ToArrayAsync();
            taskModels.Add(CreateTriggeredJobTaskModel(t.Definition, t.Task, entries));
        }
        return taskModels.ToArray();
    }

    private static TriggeredJobWithTasksModel CreateTriggeredJobDetailModel(TriggeredJobEntity jobEntity, JobDefinitionEntity jobDefEntity, TriggeredJobTaskModel[] tasks) =>
        new TriggeredJobWithTasksModel
        (
            new TriggeredJobModel
            (
                jobEntity.ID,
                new JobDefinitionModel
                (
                    jobDefEntity.ID,
                    new JobKey(jobDefEntity.DisplayText)
                ),
                jobEntity.EventNotificationID
            ),
            tasks
        );

    private static TriggeredJobTaskModel CreateTriggeredJobTaskModel(JobTaskDefinitionEntity taskDefEntity, TriggeredJobTaskEntity taskEntity, IEnumerable<LogEntryEntity> entries) =>
        new TriggeredJobTaskModel
        (
            taskEntity.ID,
            new JobTaskDefinitionModel(taskDefEntity.ID, new JobTaskKey(taskDefEntity.DisplayText)),
            JobTaskStatus.Values.Value(taskEntity.Status),
            taskEntity.Generation,
            taskEntity.TimeStarted,
            taskEntity.TimeEnded,
            taskEntity.TaskData,
            entries
                .Select
                (
                    e => new JobLogEntryModel
                    (
                        e.ID,
                        AppEventSeverity.Values.Value(e.Severity),
                        e.TimeOccurred,
                        e.Category,
                        e.Message,
                        e.Details,
                        e.SourceLogEntryKey
                    )
                )
                .ToArray()
        );

}
