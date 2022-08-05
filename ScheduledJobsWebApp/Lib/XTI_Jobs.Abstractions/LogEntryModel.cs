using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed record class LogEntryModel(int ID, AppEventSeverity Severity, DateTimeOffset TimeOccurred, string Category, string Message, string Details);
