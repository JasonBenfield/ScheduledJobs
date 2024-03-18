namespace XTI_Jobs.Abstractions;

public sealed class RetryJobsRequest
{
    public RetryJobsRequest()
        : this(new EventKey(), new JobKey())
    {
    }

    public RetryJobsRequest(EventKey eventKey, JobKey jobKey)
    {
        EventKey = eventKey.DisplayText;
        JobKey = jobKey.DisplayText;
    }

    public string EventKey { get; set; }
    public string JobKey { get; set; }
}
