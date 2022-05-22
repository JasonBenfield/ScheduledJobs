namespace XTI_Jobs;

public sealed class JobActionResult
{
    internal JobActionResult(TriggeredJobTaskModel completedTask, NextTaskModel[] nextTasks)
    {
        CompletedTask = completedTask;
        NextTasks = nextTasks;
    }

    public TriggeredJobTaskModel CompletedTask { get; }
    public NextTaskModel[] NextTasks { get; }
}