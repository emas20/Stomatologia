using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stomatologia.Migrations
{
    public partial class stomatolog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specjalizacja",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UmowWizyteViewModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Imie",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Nazwisko",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wizyty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WybranaData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WybranaGodzina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WybranyStomatologId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wizyty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wizyty_AspNetUsers_WybranyStomatologId",
                        column: x => x.WybranyStomatologId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UmowWizyteViewModelId",
                table: "AspNetUsers",
                column: "UmowWizyteViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_WybranyStomatologId",
                table: "Wizyty",
                column: "WybranyStomatologId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wizyty_UmowWizyteViewModelId",
                table: "AspNetUsers",
                column: "UmowWizyteViewModelId",
                principalTable: "Wizyty",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wizyty_UmowWizyteViewModelId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Wizyty");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UmowWizyteViewModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Specjalizacja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UmowWizyteViewModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "User_Imie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "User_Nazwisko",
                table: "AspNetUsers");
        }
    }
}
