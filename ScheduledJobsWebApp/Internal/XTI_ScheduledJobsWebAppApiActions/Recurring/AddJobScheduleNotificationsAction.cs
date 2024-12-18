using Microsoft.EntityFrameworkCore;
using XTI_Core;
using XTI_Schedule;

namespace XTI_ScheduledJobsWebAppApiActions.Recurring;

public sealed class AddJobScheduleNotificationsAction : AppAction<EmptyRequest, EmptyActionResult>
{
    private readonly JobDbContext db;
    private readonly IClock clock;

    public AddJobScheduleNotificationsAction(JobDbContext db, IClock clock)
    {
        this.db = db;
        this.clock = clock;
    }

    public async Task<EmptyActionResult> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        var schedules = await db.JobSchedules.Retrieve().ToArrayAsync();
        foreach (var schedule in schedules)
        {
            await db.Transaction(() => AddNotifications(schedule));
        }
        return new EmptyActionResult();
    }

    private async Task AddNotifications(JobScheduleEntity schedule)
    {
        var jobDef = await db.JobDefinitions.Retrieve()
            .Where(jd => jd.ID == schedule.JobDefinitionID)
            .FirstAsync();
        var jobKey = new JobKey(jobDef.DisplayText);
        var eventKey = EventKey.Scheduled(jobKey);
        var evtDef = await db.EventDefinitions.Retrieve()
            .Where(ed => ed.EventKey == eventKey.Value)
            .FirstOrDefaultAsync();
        if (evtDef == null)
        {
            throw new ArgumentException($"Event Definition '{eventKey.DisplayText}' not found");
        }
        var aggregateSchedule = AggregateSchedule.Deserialize(schedule.Serialized);
        var minTime = clock.Now().Add(TimeSpan.FromMinutes(5));
        var dateTimeRanges = aggregateSchedule.DateTimeRanges(DateRange.From(clock.Now().Date).ForOneDay())
            .Where(dtr => dtr.Start >= minTime)
            .ToArray();
        await new EfEventNotificationCreator(db, clock).AddJobScheduleNotifications(evtDef, dateTimeRanges);
    }
}
