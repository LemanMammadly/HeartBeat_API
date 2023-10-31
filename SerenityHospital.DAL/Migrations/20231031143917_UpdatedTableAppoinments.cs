using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class UpdatedTableAppoinments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Appoinments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appoinments_DepartmentId",
                table: "Appoinments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinments_Departments_DepartmentId",
                table: "Appoinments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoinments_Departments_DepartmentId",
                table: "Appoinments");

            migrationBuilder.DropIndex(
                name: "IX_Appoinments_DepartmentId",
                table: "Appoinments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Appoinments");
        }
    }
}
