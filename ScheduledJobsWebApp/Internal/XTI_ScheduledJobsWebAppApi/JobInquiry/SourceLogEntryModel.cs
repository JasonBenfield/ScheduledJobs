using XTI_Hub.Abstractions;

namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed record SourceLogEntryModel(int LogEntryID, AppLogEntryModel SourceLogEntry)
{
    public SourceLogEntryModel()
        : this(0, new AppLogEntryModel())
    {
    }
}
