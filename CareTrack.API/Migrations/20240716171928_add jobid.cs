using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class addjobid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Shifts");
        }
    }
}
