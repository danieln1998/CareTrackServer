using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class addstatustoshiftAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ShiftAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShiftAssignments");
        }
    }
}
