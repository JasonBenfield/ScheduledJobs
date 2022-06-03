using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTI_JobsDB.SqlServer.Migrations
{
    public partial class IndexTasksbyJobIDandSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TriggeredJobTasks_TriggeredJobID",
                table: "TriggeredJobTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobTasks_TriggeredJobID_Sequence",
                table: "TriggeredJobTasks",
                columns: new[] { "TriggeredJobID", "Sequence" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TriggeredJobTasks_TriggeredJobID_Sequence",
                table: "TriggeredJobTasks");

            migrationBuilder.CreateIndex(
                name: "IX_TriggeredJobTasks_TriggeredJobID",
                table: "TriggeredJobTasks",
                column: "TriggeredJobID");
        }
    }
}
