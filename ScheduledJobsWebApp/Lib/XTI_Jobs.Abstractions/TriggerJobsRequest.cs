namespace XTI_Jobs.Abstractions;

public sealed class TriggerJobsRequest
{
    public TriggerJobsRequest()
        :this(new EventKey(), new JobKey(), DateTimeOffset.MaxValue)
    {    
    }

    public TriggerJobsRequest(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime)
    {
        EventKey = eventKey.DisplayText;
        JobKey = jobKey.DisplayText;
        EventRaisedStartTime = eventRaisedStartTime;
    }

    public string EventKey { get; set; }
    public string JobKey { get; set; }
    public DateTimeOffset EventRaisedStartTime { get; set; } 
}
