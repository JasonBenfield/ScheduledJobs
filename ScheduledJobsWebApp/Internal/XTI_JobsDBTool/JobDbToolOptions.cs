using XTI_Core;

namespace XTI_JobsDbTool;

internal sealed class JobDbToolOptions
{
    public string Command { get; set; } = "";
    public string BackupFilePath { get; set; } = "";
    public bool Force { get; set; }
    public DbOptions DB { get; set; } = new();
}