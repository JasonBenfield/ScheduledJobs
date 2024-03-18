namespace XTI_Jobs.Abstractions;

public sealed class DeleteJobsWithNoTasksRequest
{
    public DeleteJobsWithNoTasksRequest()
        : this(new EventKey(), new JobKey())
    {
    }

    public DeleteJobsWithNoTasksRequest(EventKey eventKey, JobKey jobKey)
    {
        EventKey = eventKey.DisplayText;
        JobKey = jobKey.DisplayText;
    }

    public string EventKey { get; set; }
    public string JobKey { get; set; }
}
