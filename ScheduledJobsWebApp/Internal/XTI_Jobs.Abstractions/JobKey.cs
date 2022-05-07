using System.ComponentModel;
using System.Text.Json.Serialization;
using XTI_Core;

namespace XTI_Jobs.Abstractions;

[JsonConverter(typeof(TextValueJsonConverter<JobKey>))]
[TypeConverter(typeof(TextValueTypeConverter<JobKey>))]
public sealed class JobKey : TextKeyValue
{
    public JobKey(string value) : base(value)
    {
    }
}
