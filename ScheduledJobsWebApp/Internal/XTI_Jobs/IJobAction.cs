namespace XTI_Jobs;

public interface IJobAction
{
    Task<JobActionResult> Execute();

    Task<JobErrorResult> OnError(Exception ex);
}
