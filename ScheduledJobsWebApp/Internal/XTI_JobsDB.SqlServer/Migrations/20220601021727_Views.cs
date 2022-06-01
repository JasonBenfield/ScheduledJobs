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
RETURNS datetime
AS 
BEGIN
    RETURN case 
		when @dt < '0001-01-02' then '1900-01-01'
		when @dt > '9999-01-01' then '9999-12-31'
		else cast(@dt at time zone 'Eastern Standard Time' as datetime) 
		end
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
	a.id EventNotificationID, b.EventKey, 
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
	b.TimeToStartNotifications
from EventNotifications a
inner join EventDefinitions b
on a.EventDefinitionID = b.id
left outer join JobCounts c
on a.id = c.EventNotificationID
"
		);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
