﻿namespace XTI_ScheduledJobsServiceAppApi.Jobs;

public sealed class JobsGroup : AppApiGroupWrapper
{
    public JobsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddJobScheduleNotifications = source.AddAction
        (
            nameof(AddJobScheduleNotifications),
            () => sp.GetRequiredService<AddJobScheduleNotificationsAction>()
        );
        PurgeJobsAndEvents = source.AddAction
        (
            nameof(PurgeJobsAndEvents), 
            () => sp.GetRequiredService<PurgeJobsAndEventsAction>()
        );
        TimeoutJobs = source.AddAction
        (
            nameof(TimeoutJobs), 
            () => sp.GetRequiredService<TimeoutJobsAction>()
        );
    }

    public AppApiAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutJobs { get; }
}