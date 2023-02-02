namespace XTI_JobsDB.EF;

public sealed class LogEntryEntity
{
    public int ID { get; set; }
    public int TaskID { get; set; }
    public int Severity { get; set; }
    public string Category { get; set; } = "";
    public string Message { get; set; } = "";
    public string Details { get; set; } = "";
    public string SourceLogEntryKey { get; set; } = "";
    public DateTimeOffset TimeOccurred { get; set; } = DateTimeOffset.MaxValue;
}
