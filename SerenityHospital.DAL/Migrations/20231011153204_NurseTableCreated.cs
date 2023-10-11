using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class NurseTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndWork",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nurse_DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nurse_Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Nurse_EndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Nurse_IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Nurse_Salary",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Nurse_StartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "DATEADD(hour, 4, GETUTCDATE())");

            migrationBuilder.AddColumn<int>(
                name: "Nurse_Status",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartWork",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Nurse_DepartmentId",
                table: "AspNetUsers",
                column: "Nurse_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_Nurse_DepartmentId",
                table: "AspNetUsers",
                column: "Nurse_DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_Nurse_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Nurse_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EndWork",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_Description",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_EndDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_Salary",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_StartDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nurse_Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartWork",
                table: "AspNetUsers");
        }
    }
}
