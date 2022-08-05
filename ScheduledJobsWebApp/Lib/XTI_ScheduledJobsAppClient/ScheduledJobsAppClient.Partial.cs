using System.Text.Json;
using XTI_Core;

namespace XTI_ScheduledJobsAppClient;

partial class ScheduledJobsAppClient
{
    protected override void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
    {
        options.AddCoreConverters();
    }
}
