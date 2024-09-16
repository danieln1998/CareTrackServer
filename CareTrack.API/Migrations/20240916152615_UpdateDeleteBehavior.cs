using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Rooms_RoomId",
                table: "Patients");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Rooms_RoomId",
                table: "Patients",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Rooms_RoomId",
                table: "Patients");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Devices_DeviceId",
                table: "Patients",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Rooms_RoomId",
                table: "Patients",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
