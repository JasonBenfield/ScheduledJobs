using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class JobStatus : NumericValue, IEquatable<JobStatus>
{
    public sealed class JobStatuses : NumericValues<JobStatus>
    {
        internal JobStatuses() 
            : base(new JobStatus(0, nameof(NotRun)))
        {
            NotRun = DefaultValue;
            Completed = Add(new JobStatus(10, nameof(Completed)));
        }
        public JobStatus NotRun { get; }
        public JobStatus Completed { get; }
    }

    public static readonly JobStatuses Values = new JobStatuses();

    private JobStatus(int value, string displayText) : base(value, displayText)
    {
    }

    public bool Equals(JobStatus? other) => _Equals(other);
}
