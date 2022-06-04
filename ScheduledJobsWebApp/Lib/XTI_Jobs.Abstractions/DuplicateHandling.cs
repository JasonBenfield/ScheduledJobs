using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class DuplicateHandling : NumericValue, IEquatable<DuplicateHandling>
{
    public sealed class DuplicateHandlings : NumericValues<DuplicateHandling>
    {
        internal DuplicateHandlings()
            : base(new DuplicateHandling(0, nameof(Ignore)))
        {
            Ignore = DefaultValue;
            KeepOldest = Add(new DuplicateHandling(10, nameof(KeepOldest)));
            KeepNewest = Add(new DuplicateHandling(20, nameof(KeepNewest)));
            KeepAll = Add(new DuplicateHandling(30, nameof(KeepAll)));
        }
        public DuplicateHandling Ignore { get; }
        public DuplicateHandling KeepOldest { get; }
        public DuplicateHandling KeepNewest { get; }
        public DuplicateHandling KeepAll { get; }
    }

    public static readonly DuplicateHandlings Values = new DuplicateHandlings();

    private DuplicateHandling(int value, string displayText) : base(value, displayText)
    {
    }

    public bool Equals(DuplicateHandling? other) => _Equals(other);
}
