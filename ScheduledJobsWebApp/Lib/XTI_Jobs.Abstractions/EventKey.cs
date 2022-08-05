using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed class EventKey : TextKeyValue, IEquatable<EventKey>
{
    public EventKey() : this("") { }

    public EventKey(string value) : base(value)
    {
    }

    public bool Equals(EventKey? other) => _Equals(other);
}
