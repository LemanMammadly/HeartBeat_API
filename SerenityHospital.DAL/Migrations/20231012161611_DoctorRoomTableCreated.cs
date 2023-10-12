using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenityHospital.DAL.Migrations
{
    public partial class DoctorRoomTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorRoomId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorRooms_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DoctorRoomId",
                table: "AspNetUsers",
                column: "DoctorRoomId",
                unique: true,
                filter: "[DoctorRoomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorRooms_DepartmentId",
                table: "DoctorRooms",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorRooms_Number",
                table: "DoctorRooms",
                column: "Number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DoctorRooms_DoctorRoomId",
                table: "AspNetUsers",
                column: "DoctorRoomId",
                principalTable: "DoctorRooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DoctorRooms_DoctorRoomId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DoctorRooms");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DoctorRoomId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DoctorRoomId",
                table: "AspNetUsers");
        }
    }
}
