using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autenticacao.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                 table: "Usuarios",
                 columns: new[] { "Id", "Nome", "Email", "SenhaHash" },
                 values: new object[] {
                     Guid.Parse("7f6aeedf-b6ac-45a2-ba3f-3596123ffef5"),
                     "Administrador",
                     "admin@admin.com",
                     BCrypt.Net.BCrypt.HashPassword("admin")
                 });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
