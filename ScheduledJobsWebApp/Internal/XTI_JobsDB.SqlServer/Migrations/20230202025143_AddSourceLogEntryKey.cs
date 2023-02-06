using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTIJobsDB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceLogEntryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceLogEntryKey",
                table: "LogEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceLogEntryKey",
                table: "LogEntries");
        }
    }
}
