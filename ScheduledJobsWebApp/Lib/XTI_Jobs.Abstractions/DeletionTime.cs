using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class DeletionTime : NumericValue, IEquatable<DeletionTime>
{
    public sealed class DeleteAfterCancels : NumericValues<DeletionTime>
    {
        internal DeleteAfterCancels() : base(new DeletionTime(0, nameof(JobDefault)))
        {
            JobDefault = DefaultValue;
            Immediately = Add(new DeletionTime(10, nameof(Immediately)));
            NextDay = Add(new DeletionTime(20, nameof(NextDay)));
        }

        public DeletionTime JobDefault { get; }
        public DeletionTime Immediately { get; }
        public DeletionTime NextDay { get; }
    }

    public static readonly DeleteAfterCancels Values = new DeleteAfterCancels();

    private DeletionTime(int value, string displayText) : base(value, displayText)
    {
    }

    public bool Equals(DeletionTime? other) => _Equals(other);
}
