namespace XTI_Jobs;

public sealed class JobBuilder
{
    private readonly JobKey jobKey;

    internal JobBuilder(JobKey jobKey)
    {
        this.jobKey = jobKey;
    }

    public RegisteredJob Build() => new RegisteredJob(jobKey);
}
