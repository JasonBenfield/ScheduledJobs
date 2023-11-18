using XTI_Core;
using XTI_Hub.Abstractions;
using XTI_HubAppClient;

namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class GetJobDetailAction : AppAction<GetJobDetailRequest, TriggeredJobDetailModel>
{
    private readonly JobDbContext db;
    private readonly HubAppClient hubClient;

    public GetJobDetailAction(JobDbContext db, HubAppClient hubClient)
    {
        this.db = db;
        this.hubClient = hubClient;
    }

    public async Task<TriggeredJobDetailModel> Execute(GetJobDetailRequest model, CancellationToken stoppingToken)
    {
        var jobWithTasks = await new EfTriggeredJobDetail(db, model.JobID).Value();
        var inquiry = new EfEventNotificationInquiry(db);
        var triggeredBy = await inquiry.Notification(jobWithTasks.Job.EventNotificationID);
        var logEntriesWithSource = jobWithTasks.Tasks
            .SelectMany(t => t.LogEntries)
            .Where(le => !string.IsNullOrWhiteSpace(le.SourceEventKey))
            .ToArray();
        var sourceLogEntries = new List<SourceLogEntryModel>();
        foreach (var logEntry in logEntriesWithSource)
        {
            try
            {
                var sourceLogEntry = await hubClient.Logs.GetLogEntryOrDefaultByKey(logEntry.SourceEventKey, stoppingToken);
                sourceLogEntries.Add(new SourceLogEntryModel(logEntry.ID, sourceLogEntry));
            }
            catch
            {
                sourceLogEntries.Add
                (
                    new SourceLogEntryModel
                    (
                        0, 
                        new AppLogEntryModel
                        (
                            ID: 0,
                            RequestID: 0,
                            TimeOccurred: DateTimeOffset.MaxValue,
                            Severity: AppEventSeverity.Values.NotSet,
                            Caption: "",
                            Message: "Placeholder",
                            Detail: "",
                            Category: ""
                        )
                    )
                );
            }
        }
        return new TriggeredJobDetailModel
        (
            jobWithTasks.Job, 
            triggeredBy, 
            jobWithTasks.Tasks, 
            sourceLogEntries.ToArray()
        );
    }
}
