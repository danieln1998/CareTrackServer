using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class changetoUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_IdentityUser_UserIdId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "UserIdId",
                table: "Employees",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_UserIdId",
                table: "Employees",
                newName: "IX_Employees_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_IdentityUser_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_IdentityUser_UserId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Employees",
                newName: "UserIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                newName: "IX_Employees_UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_IdentityUser_UserIdId",
                table: "Employees",
                column: "UserIdId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
