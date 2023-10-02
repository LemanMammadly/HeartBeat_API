using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class PatientRoomTableCreatedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PatientRooms_Number",
                table: "PatientRooms",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PatientRooms_Number",
                table: "PatientRooms");
        }
    }
}
