using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class JobTaskStatus : NumericValue, IEquatable<JobTaskStatus>
{
    public sealed class JobStatuses : NumericValues<JobTaskStatus>
    {
        internal JobStatuses() 
            : base(new JobTaskStatus(0, nameof(NotRun)))
        {
            NotRun = DefaultValue;
            Failed = Add(new JobTaskStatus(10, nameof(Failed)));
            Retry = Add(new JobTaskStatus(20, nameof(Retry)));
            Running = Add(new JobTaskStatus(30, nameof(Running)));
            Pending = Add(new JobTaskStatus(40, nameof(Pending)));
            Canceled = Add(new JobTaskStatus(50, nameof(Canceled)));
            Completed = Add(new JobTaskStatus(60, nameof(Completed)));
        }
        public JobTaskStatus NotRun { get; }
        public JobTaskStatus Failed { get; }
        public JobTaskStatus Retry { get; }
        public JobTaskStatus Running { get; }
        public JobTaskStatus Pending { get; }
        public JobTaskStatus Canceled { get; }
        public JobTaskStatus Completed { get; }
    }

    public static readonly JobStatuses Values = new JobStatuses();

    private JobTaskStatus(int value, string displayText) : base(value, displayText)
    {
    }

    public bool Equals(JobTaskStatus? other) => _Equals(other);
}
