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
                    CompareSourceKeyAndDataForDuplication = table.Column<bool>(type: "bit", nullable: false),
                    DuplicateHandling = table.Column<int>(type: "int", nullable: false),
                    TimeToStartNotifications = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActiveFor = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Timeout = table.Column<string>(type: "nvarchar(48)", nullable: false)
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
                    TimeInactive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                    TimeInactive = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                name: "IX_TriggeredJobTasks_TriggeredJobID",
                table: "TriggeredJobTasks",
                column: "TriggeredJobID");
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
