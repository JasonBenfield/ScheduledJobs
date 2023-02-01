using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class EventKey : TextKeyValue, IEquatable<EventKey>
{
    public static EventKey OnDemand(JobKey jobKey) => new($"[On Demand] {jobKey.DisplayText}");

    public static EventKey Scheduled(JobKey jobKey) => new($"[Scheduled] {jobKey.DisplayText}");

    public EventKey() : this("") { }

    public EventKey(string value) : base(value)
    {
    }

    public bool IsOnDemand() => DisplayText.StartsWith("[On Demand]", StringComparison.OrdinalIgnoreCase);

    public bool IsScheduled() => DisplayText.StartsWith("[Scheduled]", StringComparison.OrdinalIgnoreCase);

    public bool Equals(EventKey? other) => _Equals(other);
}
