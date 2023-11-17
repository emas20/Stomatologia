using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stomatologia.Data.Migrations
{
    public partial class identityandprofileview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EditProfileViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConfirmNewPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditProfileViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EditProfileViewModel_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditProfileViewModel_IdentityUserId",
                table: "EditProfileViewModel",
                column: "IdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditProfileViewModel");
        }
    }
}
