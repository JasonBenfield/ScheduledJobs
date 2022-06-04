using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTI_JobsDB.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventDefinitions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompareSourceKeyAndDataForDuplication = table.Column<bool>(type: "bit", nullable: false),
                    DuplicateHandling = table.Column<int>(type: "int", nullable: false),
                    TimeToStartNotifications = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActiveFor = table.Column<string>(type: "nvarchar(48)", nullable: false),
                    DeleteAfter = table.Column<string>(type: "nvarchar(48)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JobDefinitions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timeout = table.Column<string>(type: "nvarchar(48)", nullable: false),
                    DeleteAfter = table.Column<string>(type: "nvarchar(48)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JobTaskDefinitions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionID = table.Column<int>(type: "int", nullable: false),
                    TaskKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Timeout = table.Column<string>(type: "nvarchar(48)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTaskDefinitions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventNotifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventDefinitionID = table.Column<int>(type: "int", nullable: false),
                    SourceKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeAdded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeActive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeInactive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeToDelete = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EventNotifications_EventDefinitions_EventDefinitionID",
                        column: x => x.EventDefinitionID,
                        principalTable: "EventDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TriggeredJobs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionID = table.Column<int>(type: "int", nullable: false),
                    EventNotificationID = table.Column<int>(type: "int", nullable: false),
                    TimeInactive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeToDelete = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggeredJobs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TriggeredJobs_EventNotifications_EventNotificationID",
                        column: x => x.EventNotificationID,
                        principalTable: "EventNotifications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TriggeredJobs_JobDefinitions_JobDefinitionID",
                        column: x => x.JobDefinitionID,
                        principalTable: "JobDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TriggeredJobTasks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriggeredJobID = table.Column<int>(type: "int", nullable: false),
                    TaskDefinitionID = table.Column<int>(type: "int", nullable: false),
                    Generation = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeActive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeInactive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeStarted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TimeEnded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggeredJobTasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TriggeredJobTasks_JobTaskDefinitions_TaskDefinitionID",
                        column: x => x.TaskDefinitionID,
                        principalTable: "JobTaskDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TriggeredJobTasks_TriggeredJobs_TriggeredJobID",
                        column: x => x.TriggeredJobID,
                        principalTable: "TriggeredJobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HierarchicalTriggeredJobTasks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentTaskID = table.Column<int>(type: "int", nullable: false),
                    ChildTaskID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierarchicalTriggeredJobTasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HierarchicalTriggeredJobTasks_TriggeredJobTasks_ChildTaskID",
                        column: x => x.ChildTaskID,
                        principalTable: "TriggeredJobTasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HierarchicalTriggeredJobTasks_TriggeredJobTasks_ParentTaskID",
                        column: x => x.ParentTaskID,
                        principalTable: "TriggeredJobTasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskID = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeOccurred = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogEntries_TriggeredJobTasks_TaskID",
                        column: x => x.TaskID,
                        principalTable: "TriggeredJobTasks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventDefinitions_EventKey",
                table: "EventDefinitions",
                column: "EventKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventNotifications_EventDefinitionID",
                table: "EventNotifications",
                column: "EventDefinitionID");

            migrationBuilder.CreateIndex(
                name: "IX_HierarchicalTriggeredJobTasks_ChildTaskID",
                table: "HierarchicalTriggeredJobTasks",
                column: "ChildTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_HierarchicalTriggeredJobTasks_ParentTaskID_ChildTaskID",
                table: "HierarchicalTriggeredJobTasks",
                columns: new[] { "ParentTaskID", "ChildTaskID" });

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_JobKey",
                table: "JobDefinitions",
                column: "JobKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobTaskDefinitions_JobDefinitionID_TaskKey",
                table: "JobTaskDefinitions",
                columns: new[] { "JobDefinitionID", "TaskKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_TaskID",
                table: "LogEntries",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobs_EventNotificationID",
                table: "TriggeredJobs",
                column: "EventNotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobs_JobDefinitionID",
                table: "TriggeredJobs",
                column: "JobDefinitionID");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobTasks_TaskDefinitionID",
                table: "TriggeredJobTasks",
                column: "TaskDefinitionID");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobTasks_TriggeredJobID_Sequence",
                table: "TriggeredJobTasks",
                columns: new[] { "TriggeredJobID", "Sequence" });

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
	dbo.JobTaskStatusDisplayText(isnull(JobStatuses.JobStatus,0)) JobStatusDisplayText,
	dbo.ToEst(isnull(StartTimes.TimeStarted,'1/1/1')) TimeJobStartedEst,
	dbo.ToEst(isnull(EndTimes.TimeEnded,'12/31/9999')) TimeJobEndedEst,
	dbo.TimeElapsedDisplayText(isnull(StartTimes.TimeStarted,'1/1/1'), isnull(EndTimes.TimeEnded,'12/31/9999')) TimeElapsed,
	isnull(TaskCounts.TaskCount, 0) TaskCount,
	jobs.JobDefinitionID,
	dbo.ToEst(jobs.TimeInactive) TimeJobInactiveEst, 
	jobDefs.JobKey, jobDefs.Timeout JobTimeout,
	evtDefs.DisplayText EventDisplayText, evtDefs.EventKey, evtNots.SourceKey, evtNots.SourceData,
	evtNots.ID EventNotificationID, evtDefs.ID EventDefinitionID,
	dbo.ToEst(evtNots.TimeAdded) TimeEventAddedEst,
	dbo.ToEst(evtNOts.TimeActive) TimeEventActiveEst,
	dbo.ToEst(evtNots.TimeInactive) TimeEventInactiveEst,
	evtNots.TimeAdded TimeEventAdded, evtNots.TimeActive TimeEventActive, evtNots.TimeInactive TimeEventInactive,
	isnull(JobStatuses.JobStatus,0) JobStatus,
	jobs.TimeInactive TimeJobInactive,
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
            migrationBuilder.Sql
            (
                @"
create or alter view ExpandedLogEntries as
select 
	entries.ID LogEntryID, 
	case entries.Severity when 100 then 'Error' else 'Information' end SeverityDispalyText,
	entries.Category, entries.Message, entries.Details, 
	dbo.ToEst(entries.TimeOccurred) TimeOccurredEst,
	entries.Severity, entries.TimeOccurred,
	tasks.ID TaskID, tasks.Generation, 
	tasks.Sequence,
	taskDefs.DisplayText TaskDisplayText,
	dbo.JobTaskStatusDisplayText(tasks.Status) TaskStatusDisplayText,
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
from LogEntries entries
inner join TriggeredJobTasks tasks
on entries.TaskID = tasks.ID
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
"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HierarchicalTriggeredJobTasks");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "TriggeredJobTasks");

            migrationBuilder.DropTable(
                name: "JobTaskDefinitions");

            migrationBuilder.DropTable(
                name: "TriggeredJobs");

            migrationBuilder.DropTable(
                name: "EventNotifications");

            migrationBuilder.DropTable(
                name: "JobDefinitions");

            migrationBuilder.DropTable(
                name: "EventDefinitions");
        }
    }
}
