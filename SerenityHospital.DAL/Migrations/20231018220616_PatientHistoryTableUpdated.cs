using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class PatientHistoryTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "PatientHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "PatientHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
