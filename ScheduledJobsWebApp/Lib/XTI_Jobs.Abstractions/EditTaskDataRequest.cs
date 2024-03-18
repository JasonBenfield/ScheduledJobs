namespace XTI_Jobs.Abstractions;

public sealed class EditTaskDataRequest
{
    public EditTaskDataRequest()
        : this(0, "")
    {
    }

    public EditTaskDataRequest(int taskID, string taskData)
    {
        TaskID = taskID;
        TaskData = taskData;
    }

    public int TaskID { get; set; }
    public string TaskData { get; set; }
}
