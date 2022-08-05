using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private TasksGroup? _Tasks;

    public TasksGroup Tasks { get => _Tasks ?? throw new ArgumentNullException(nameof(_Tasks)); }

    partial void createTasksGroup(IServiceProvider sp)
    {
        _Tasks = new TasksGroup
        (
            source.AddGroup(nameof(Tasks)),
            sp
        );
    }
}