using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTI_JobsDB.SqlServer.Migrations
{
    public partial class PurgeJobsandEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeToDelete",
                table: "TriggeredJobs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "DeleteAfter",
                table: "JobDefinitions",
                type: "nvarchar(48)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeToDelete",
                table: "EventNotifications",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "ActiveFor",
                table: "EventDefinitions",
                type: "nvarchar(48)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DeleteAfter",
                table: "EventDefinitions",
                type: "nvarchar(48)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToDelete",
                table: "TriggeredJobs");

            migrationBuilder.DropColumn(
                name: "DeleteAfter",
                table: "JobDefinitions");

            migrationBuilder.DropColumn(
                name: "TimeToDelete",
                table: "EventNotifications");

            migrationBuilder.DropColumn(
                name: "DeleteAfter",
                table: "EventDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "ActiveFor",
                table: "EventDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(48)");
        }
    }
}
