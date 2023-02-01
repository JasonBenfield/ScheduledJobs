using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XTIJobsDB.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddJobSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobSchedules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDefinitionID = table.Column<int>(type: "int", nullable: false),
                    Serialized = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JobSchedules_JobDefinitions_JobDefinitionID",
                        column: x => x.JobDefinitionID,
                        principalTable: "JobDefinitions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSchedules_JobDefinitionID",
                table: "JobSchedules",
                column: "JobDefinitionID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSchedules");
        }
    }
}
