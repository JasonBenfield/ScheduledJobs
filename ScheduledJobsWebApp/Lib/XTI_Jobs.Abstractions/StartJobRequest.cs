namespace XTI_Jobs.Abstractions;

public sealed class StartJobRequest
{
    public StartJobRequest()
        : this(0, [])
    {
    }

    public StartJobRequest(int jobID, params NextTaskModel[] nextTasks)
    {
        JobID = jobID;
        NextTasks = nextTasks;
    }

    public int JobID { get; set; }
    public NextTaskModel[] NextTasks { get; set; }
}
