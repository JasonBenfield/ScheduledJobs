using XTI_Schedule;

namespace XTI_Jobs;

public sealed class JobScheduleRegistration
{
    private readonly IJobDb db;
    private readonly JobKey jobKey;
    private readonly Schedule[] schedules;
    private readonly TimeSpan deleteAfter;

    public JobScheduleRegistration(IJobDb db, JobKey jobKey, Schedule[] schedules, TimeSpan deleteAfter)
    {
        this.db = db;
        this.jobKey = jobKey;
        this.schedules = schedules;
        this.deleteAfter = deleteAfter;
    }

    public Task Register() =>
        db.AddOrUpdateJobSchedules
        (
            jobKey,
            new AggregateSchedule(schedules),
            deleteAfter
        );
}

public sealed class JobScheduleRegistrationBuilder
{
    private readonly IJobDb db;
    private JobKey? jobKey;
    private Schedule[]? schedules;
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);

    public JobScheduleRegistrationBuilder(IJobDb db)
    {
        this.db = db;
    }

    public JobScheduleRegistrationBuilder1 Trigger(JobKey jobKey)
    {
        this.jobKey = jobKey;
        return new JobScheduleRegistrationBuilder1(this);
    }

    internal void When(Schedule[] schedules) => this.schedules = schedules;

    internal void DeleteAfter(TimeSpan deleteAfter) => this.deleteAfter = deleteAfter;

    internal JobScheduleRegistration Build() =>
        new JobScheduleRegistration
        (
            db,
            jobKey ?? throw new ArgumentNullException(nameof(jobKey)),
            schedules ?? throw new ArgumentNullException(nameof(schedules)),
            deleteAfter
        );
}

public sealed class JobScheduleRegistrationBuilder1
{
    private readonly JobScheduleRegistrationBuilder builder;

    internal JobScheduleRegistrationBuilder1(JobScheduleRegistrationBuilder builder)
    {
        this.builder = builder;
    }

    public JobScheduleRegistrationBuilderFinal When(params Schedule[] schedules)
    {
        builder.When(schedules);
        return new JobScheduleRegistrationBuilderFinal(builder);
    }
}

public sealed class JobScheduleRegistrationBuilderFinal
{
    private readonly JobScheduleRegistrationBuilder builder;

    internal JobScheduleRegistrationBuilderFinal(JobScheduleRegistrationBuilder builder)
    {
        this.builder = builder;
    }

    public JobScheduleRegistrationBuilderFinal DeleteAfter(TimeSpan deleteAfter)
    {
        builder.DeleteAfter(deleteAfter);
        return this;
    }

    public JobScheduleRegistration Build() => builder.Build();
}
