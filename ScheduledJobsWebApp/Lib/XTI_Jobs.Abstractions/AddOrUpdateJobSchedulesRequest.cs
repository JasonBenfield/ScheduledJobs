namespace XTI_Jobs.Abstractions;

public sealed class AddOrUpdateJobSchedulesRequest
{
    public AddOrUpdateJobSchedulesRequest()
        : this(new JobKey(), "", new TimeSpan())
    {
    }

    public AddOrUpdateJobSchedulesRequest(JobKey jobKey, string schedules, TimeSpan deleteAfter)
    {
        JobKey = jobKey.DisplayText;
        Schedules = schedules;
        DeleteAfter = deleteAfter;
    }

    public string JobKey { get; set; }
    public string Schedules { get; set; }
    public TimeSpan DeleteAfter { get; set; }
}
