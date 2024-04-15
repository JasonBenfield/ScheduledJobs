namespace XTI_Jobs.Abstractions;

public sealed class LogMessageRequest
{
    public LogMessageRequest()
        : this(0, "", "", "")
    {
    }

    public LogMessageRequest(int taskID, string category, string message, string details)
    {
        TaskID = taskID;
        Category = category;
        Message = message;
        Details = details;
    }

    public int TaskID { get; set; }
    public string Category { get; set; } = "";
    public string Message { get; set; } = "";
    public string Details { get; set; } = "";
}
