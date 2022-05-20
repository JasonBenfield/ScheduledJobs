using XTI_Core;

namespace XTI_Jobs;

public sealed record NextTask
{
    private readonly NextTaskModel model;

    public NextTask(JobTaskKey taskKey, object? taskData)
    {
        var serialized = XtiSerializer.Serialize(taskData);
        model = new NextTaskModel(taskKey, serialized);
    }

    public NextTaskModel ToModel() => model;

    public override string ToString() => $"{GetType().Name} {model}";
}
