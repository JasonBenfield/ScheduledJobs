using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class JobTaskKey : TextKeyValue, IEquatable<JobTaskKey>
{
    public static readonly JobTaskKey None = new JobTaskKey("");

    public JobTaskKey(string value) : base(value)
    {
    }

    public bool Equals(JobTaskKey? other) => _Equals(other);
}
