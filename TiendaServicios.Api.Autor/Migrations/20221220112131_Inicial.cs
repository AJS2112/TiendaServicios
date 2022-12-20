using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiendaServicios.Api.Autor.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoresLibros",
                columns: table => new
                {
                    AutorLibroId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutorLibroGuid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLibros", x => x.AutorLibroId);
                });

            migrationBuilder.CreateTable(
                name: "GradosAcademicos",
                columns: table => new
                {
                    GradoAcademicoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    CentroAcademico = table.Column<string>(type: "text", nullable: false),
                    FechaGrado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutorLibroId = table.Column<int>(type: "integer", nullable: false),
                    GradoAcademicoGuid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradosAcademicos", x => x.GradoAcademicoId);
                    table.ForeignKey(
                        name: "FK_GradosAcademicos_AutoresLibros_AutorLibroId",
                        column: x => x.AutorLibroId,
                        principalTable: "AutoresLibros",
                        principalColumn: "AutorLibroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradosAcademicos_AutorLibroId",
                table: "GradosAcademicos",
                column: "AutorLibroId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradosAcademicos");

            migrationBuilder.DropTable(
                name: "AutoresLibros");
        }
    }
}
