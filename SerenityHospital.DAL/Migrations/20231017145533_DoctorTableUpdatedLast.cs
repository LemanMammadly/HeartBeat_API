using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class DoctorTableUpdatedLast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppoinmentAsDoctorId",
                table: "Appoinments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appoinments_AppoinmentAsDoctorId",
                table: "Appoinments",
                column: "AppoinmentAsDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinments_AspNetUsers_AppoinmentAsDoctorId",
                table: "Appoinments",
                column: "AppoinmentAsDoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoinments_AspNetUsers_AppoinmentAsDoctorId",
                table: "Appoinments");

            migrationBuilder.DropIndex(
                name: "IX_Appoinments_AppoinmentAsDoctorId",
                table: "Appoinments");

            migrationBuilder.DropColumn(
                name: "AppoinmentAsDoctorId",
                table: "Appoinments");
        }
    }
}
