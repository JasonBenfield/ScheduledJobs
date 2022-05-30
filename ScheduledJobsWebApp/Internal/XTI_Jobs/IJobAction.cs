namespace XTI_Jobs;

public interface IJobAction
{
    Task<JobActionResult> Execute(CancellationToken stoppingToken);

    Task<JobErrorResult> OnError(Exception ex);
}
