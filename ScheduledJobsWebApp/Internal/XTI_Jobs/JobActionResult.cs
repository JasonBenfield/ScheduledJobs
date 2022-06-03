namespace XTI_Jobs;

public sealed class JobActionResult
{
    internal JobActionResult(bool preserveData, NextTaskModel[] nextTasks)
    {
        PreserveData = preserveData;
        NextTasks = nextTasks;
    }

    public bool PreserveData { get; }
    public NextTaskModel[] NextTasks { get; }
}