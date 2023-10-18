using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class PatientHistoryTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientHistories_AspNetUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientHistories_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientHistories_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientHistories_DoctorId",
                table: "PatientHistories",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHistories_PatientId",
                table: "PatientHistories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHistories_RecipeId",
                table: "PatientHistories",
                column: "RecipeId",
                unique: true,
                filter: "[RecipeId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientHistories");
        }
    }
}
