namespace XTI_Jobs.Abstractions;

public sealed class GetJobDetailRequest
{
    public GetJobDetailRequest()
        : this(0)
    {
    }

    public GetJobDetailRequest(int jobID)
    {
        JobID = jobID;
    }

    public int JobID { get; set; }
}
