using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTI_JobsDB.SqlServer.Migrations;

public partial class Views : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql
        (
            @"
CREATE or ALTER FUNCTION [ToEST](
    @dt datetimeoffset
)
RETURNS datetime2
AS 
BEGIN
    RETURN cast(@dt at time zone 'Eastern Standard Time' as datetime2) 
END
"
        );
        migrationBuilder.Sql
        (
            @"
CREATE or ALTER FUNCTION [JobTaskStatusDisplayText](
    @type INT
)
RETURNS varchar(50)
AS 
BEGIN
    RETURN case @type 
		when 0 then 'Not Set'
		when 10 then 'Failed' 
		when 20 then 'Retry' 
		when 30 then 'Running' 
		when 40 then 'Pending' 
		when 50 then 'Canceled' 
		when 60 then 'Completed' 
		else 'Unknown' 
		end
END
"
        );
        migrationBuilder.Sql
        (
            @"
CREATE or ALTER FUNCTION [DuplicateHandlingDisplayText](
    @type INT
)
RETURNS varchar(50)
AS 
BEGIN
    RETURN case @type 
		when 0 then 'Ignore'
		when 10 then 'Keep Oldest' 
		when 20 then 'Keep Newest' 
		when 30 then 'Keep All' 
		else 'Unknown' 
		end
END
"
        );
		migrationBuilder.Sql
		(
			@"
CREATE or ALTER FUNCTION [TimeElapsedDisplayText](
    @timestarted datetimeoffset,
	@timeended datetimeoffset
)
RETURNS varchar(50)
AS 
BEGIN
    RETURN 
	case 
	when @timeended >= '9999-12-31' then null
	when datediff(year,@timestarted,@timeended) > 1 then cast(datediff(month, @TimeStarted, @TimeEnded) as varchar) + ' month'
	when datediff(day,@timestarted,@timeended) > 7 then format((datediff(hour, @TimeStarted, @TimeEnded) / 24.0), 'F2') + ' day'
	when datediff(hour,@timestarted,@timeended) > 1 then format((datediff(minute, @TimeStarted, @TimeEnded) / 60.0), 'F2') + ' hr'
	when datediff(minute,@timestarted,@timeended) > 1 then format((datediff(second, @TimeStarted, @TimeEnded) / 60.0), 'F2') + ' min'
	when datediff(second,@timestarted,@timeended) > 1 then format((datediff(millisecond, @TimeStarted, @TimeEnded) / 1000.0), 'F3') + ' s'
	else cast(datediff(millisecond, @TimeStarted, @TimeEnded) as varchar)  + ' ms'
	end
END
"
		);
        migrationBuilder.Sql
        (
			@"
create or alter view ExpandedEventNotifications 
as
with
JobCounts as
(
	select EventNotificationID, count(EventNotificationID) JobCount
	from TriggeredJobs
	group by EventNotificationID
)
select 
	a.id EventNotificationID, b.DisplayText EventDisplayText,
	dbo.ToEST(a.timeadded) TimeAddedEst, 
	isnull(c.JobCount,0) TriggeredJobCount,
	a.SourceKey, a.sourcedata, 
	dbo.ToEST(a.timeactive) TimeActiveEst, 
	dbo.ToEST(a.TimeInactive) TimeInactiveEst, 
	b.id EventDefinitionID, 
	dbo.ToEST(b.TimeToStartNotifications) TimeToStartNotificationsEst, 
	b.ActiveFor, 
	b.CompareSourceKeyAndDataForDuplication, 
	dbo.DuplicateHandlingDisplayText(b.DuplicateHandling) DuplicateHandlingDisplayText, 
	b.DuplicateHandling, 
	a.timeadded, 
	a.timeactive, 
	a.TimeInactive, 
	b.EventKey, 
	b.TimeToStartNotifications
from EventNotifications a
inner join EventDefinitions b
on a.EventDefinitionID = b.id
left outer join JobCounts c
on a.id = c.EventNotificationID
"
		);
		migrationBuilder.Sql
		(
			@"
create or alter view ExpandedTriggeredJobTasks as
with 
LogEntrySeverityCounts as
(
	select TaskID, Severity, count(TaskID) LogEntryCount
	from LogEntries
	group by TaskID, Severity
),
LogEntryCounts as
(
	select TaskID, count(TaskID) LogEntryCount
	from LogEntries
	group by TaskID
),
ChildTaskCounts as
(
	select ParentTaskID, count(ParentTaskID) ChildTaskCount
	from HierarchicalTriggeredJobTasks
	group by ParentTaskID
)
select 
	tasks.ID TaskID, tasks.Generation, 
	tasks.Sequence,
	taskDefs.DisplayText TaskDisplayText,
	dbo.JobTaskStatusDisplayText(tasks.Status) TaskStatusDisplayText,
	isnull(errorCounts.LogEntryCount, 0) ErrorCount,
	isnull(infoCounts.LogEntryCount, 0) InformationCount,
	isnull(entryCounts.LogEntryCount, 0) LogEntryCount,
	isnull(childCounts.ChildTaskCount, 0) ChildTaskCount,
	tasks.TaskData,
	dbo.ToEst(tasks.TimeStarted) TimeTaskStartedEst,
	dbo.ToEst(tasks.TimeEnded) TimeTaskEndedEst,
	dbo.TimeElapsedDisplayText(tasks.TimeStarted, tasks.TimeEnded) TimeElapsed,
	dbo.ToEst(tasks.TimeAdded) TimeTaskAddedEst,
	dbo.ToEst(tasks.TimeActive) TimeTaskActiveEst,
	dbo.ToEst(tasks.TimeInactive) TimeTaskInactiveEst,
	isnull(hierTasks.ParentTaskID,0) ParentTaskID,
	jobDefs.DisplayText JobDisplayText,
	evtDefs.DisplayText EventDisplayText, evtDefs.EventKey, evtNots.SourceKey, evtNots.SourceData,
	evtNots.ID EventNotificationID, evtDefs.ID EventDefinitionID,
	dbo.ToEst(evtNots.TimeAdded) TimeEventAddedEst,
	dbo.ToEst(evtNOts.TimeActive) TimeEventActiveEst,
	dbo.ToEst(evtNots.TimeInactive) TimeEventInactiveEst,
	jobs.ID JobID, jobDefs.ID JobDefinitionID,
	jobDefs.JobKey, jobDefs.Timeout JobTimeout,
	dbo.ToEst(jobs.TimeInactive) TimeJobInactiveEst,
	jobs.TimeInactive TimeJobInactive,
	tasks.Timestarted TimeTaskStarted, tasks.TimeEnded TimeTaskEnded, 
	tasks.TimeActive TimeTaskActive, tasks.TimeInactive TimeTaskInactive, tasks.Status TaskStatus,
	taskDefs.ID TaskDefinitionID,
	taskDefs.TaskKey, taskDefs.Timeout TaskTimeout,
	evtNots.TimeAdded TimeEventAdded, evtNots.TimeActive TimeEventActive, evtNots.TimeInactive TimeEventInactive
from TriggeredJobTasks tasks
inner join JobTaskDefinitions taskDefs
on tasks.TaskDefinitionID = taskDefs.id
inner join TriggeredJobs jobs
on tasks.TriggeredJobID = jobs.id
inner join JobDefinitions jobDefs
on jobs.JobDefinitionID = jobDefs.id
inner join EventNotifications evtNots
on jobs.EventNotificationID = evtNots.id
inner join EventDefinitions evtDefs
on evtNots.EventDefinitionID = evtDefs.id
left outer join HierarchicalTriggeredJobTasks hierTasks
on tasks.ID = hierTasks.ChildTaskID
left outer join LogEntrySeverityCounts errorCounts
on tasks.ID = errorCounts.TaskID and errorCounts.Severity = 100
left outer join LogEntrySeverityCounts infoCounts
on tasks.ID = infoCounts.TaskID and infoCounts.Severity = 50
left outer join LogEntryCounts entryCounts
on tasks.ID = entryCounts.TaskID
left outer join ChildTaskCounts childCounts
on tasks.ID = childCounts.ParentTaskID
"
		);
		migrationBuilder.Sql
		(
			@"
create or alter view ExpandedTriggeredJobs as
with 
JobStatuses as
(
	select TriggeredJobID, min(Status) JobStatus
	from TriggeredJobTasks
	group by TriggeredJobID
),
StartTimes as
(
	select TriggeredJobID, min(TimeStarted) TimeStarted
	from TriggeredJobTasks
	group by TriggeredJobID
),
EndTimes as
(
	select TriggeredJobID, max(TimeEnded) TimeEnded
	from TriggeredJobTasks
	group by TriggeredJobID
),
TaskCounts as
(
	select TriggeredJobID, count(TriggeredJobID) TaskCount
	from TriggeredJobTasks
	group by TriggeredJobID
)
select 
	jobs.ID JobID, jobDefs.DisplayText JobDisplayText,
	jobs.TimeInactive TimeJobInactive, 
	dbo.JobTaskStatusDisplayText(isnull(JobStatuses.JobStatus,0)) JobStatus,
	dbo.ToEst(isnull(StartTimes.TimeStarted,'1/1/1')) TimeJobStartedEst,
	dbo.ToEst(isnull(EndTimes.TimeEnded,'12/31/9999')) TimeJobEndedEst,
	dbo.TimeElapsedDisplayText(isnull(StartTimes.TimeStarted,'1/1/1'), isnull(EndTimes.TimeEnded,'12/31/9999')) TimeElapsed,
	isnull(TaskCounts.TaskCount, 0) TaskCount,
	jobs.JobDefinitionID, jobDefs.JobKey, jobDefs.Timeout JobTimeout,
	evtDefs.DisplayText EventDisplayText, evtDefs.EventKey, evtNots.SourceKey, evtNots.SourceData,
	evtNots.ID EventNotificationID, evtDefs.ID EventDefinitionID,
	dbo.ToEst(evtNots.TimeAdded) TimeEventAddedEst,
	dbo.ToEst(evtNOts.TimeActive) TimeEventActiveEst,
	dbo.ToEst(evtNots.TimeInactive) TimeEventInactiveEst,
	dbo.ToEst(jobs.TimeInactive) TimeJobInactiveEst,
	evtNots.TimeAdded TimeEventAdded, evtNots.TimeActive TimeEventActive, evtNots.TimeInactive TimeEventInactive,
	isnull(JobStatuses.JobStatus,0) JobStatus,
	isnull(StartTimes.TimeStarted,'1/1/1') TimeJobStarted,
	isnull(EndTimes.TimeEnded,'12/31/9999') TimeJobEnded
from TriggeredJobs jobs
inner join JobDefinitions jobDefs
on jobs.JobDefinitionID = jobDefs.id
inner join EventNotifications evtNots
on jobs.EventNotificationID = evtNots.id
inner join EventDefinitions evtDefs
on evtNots.EventDefinitionID = evtDefs.id
left outer join JobStatuses
on jobs.ID = JobStatuses.TriggeredJobID
left outer join TaskCounts
on jobs.ID = TaskCounts.TriggeredJobID
left outer join StartTimes
on jobs.ID = StartTimes.TriggeredJobID
left outer join EndTimes
on jobs.ID = EndTimes.TriggeredJobID
"
		);
    }

	protected override void Down(MigrationBuilder migrationBuilder) { }
}
