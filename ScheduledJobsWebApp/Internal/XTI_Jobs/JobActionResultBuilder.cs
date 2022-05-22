using XTI_Core;

namespace XTI_Jobs;

public sealed class JobActionResultBuilder
{
    private readonly List<NextTaskModel> nextTasks = new();
    private readonly TriggeredJobTaskModel task;

    public JobActionResultBuilder(TriggeredJobTaskModel task)
    {
        this.task = task;
    }

    public JobActionResultBuilder AddNext(JobTaskKey taskKey, object[] items)
    {
        foreach (var item in items)
        {
            AddNext(taskKey, item);
        }
        return this;
    }

    public JobActionResultBuilder AddNext(JobTaskKey taskKey, object? data)
    {
        var serialized = XtiSerializer.Serialize(data ?? new object());
        var nextTask = new NextTaskModel(taskKey, serialized);
        nextTasks.Add(nextTask);
        return this;
    }

    internal JobActionResult Build() => new JobActionResult(task, nextTasks.ToArray());
}
