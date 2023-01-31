using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class EventKey : TextKeyValue, IEquatable<EventKey>
{
    public static EventKey OnDemand(JobKey jobKey) => new EventKey($"[OnDemand] {jobKey.DisplayText}");

    public EventKey() : this("") { }

    public EventKey(string value) : base(value)
    {
    }

    public bool IsOnDemand() => Value.StartsWith("[OnDemand]", StringComparison.OrdinalIgnoreCase);

    public bool Equals(EventKey? other) => _Equals(other);
}
