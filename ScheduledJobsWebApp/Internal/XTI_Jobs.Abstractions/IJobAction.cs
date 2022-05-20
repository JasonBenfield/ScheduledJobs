namespace XTI_Jobs.Abstractions;

public interface IJobAction
{
    Task<JobActionResult> Execute();
}
