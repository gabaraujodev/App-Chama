using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanoLeitura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participantes_Grupos_GrupoId",
                table: "Participantes");

            migrationBuilder.DropForeignKey(
                name: "FK_Participantes_Usuarios_UsuarioId",
                table: "Participantes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes");

            migrationBuilder.RenameTable(
                name: "Participantes",
                newName: "Participante");

            migrationBuilder.RenameIndex(
                name: "IX_Participantes_UsuarioId",
                table: "Participante",
                newName: "IX_Participante_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participante",
                table: "Participante",
                columns: new[] { "GrupoId", "UsuarioId" });

            migrationBuilder.CreateTable(
                name: "PlanoLeitura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrupoId = table.Column<int>(type: "integer", nullable: false),
                    Livro = table.Column<string>(type: "text", nullable: false),
                    CapituloInicial = table.Column<int>(type: "integer", nullable: false),
                    CapituloFinal = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoLeitura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoLeitura_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanoLeitura_GrupoId",
                table: "PlanoLeitura",
                column: "GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participante_Grupos_GrupoId",
                table: "Participante",
                column: "GrupoId",
                principalTable: "Grupos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participante_Usuarios_UsuarioId",
                table: "Participante",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participante_Grupos_GrupoId",
                table: "Participante");

            migrationBuilder.DropForeignKey(
                name: "FK_Participante_Usuarios_UsuarioId",
                table: "Participante");

            migrationBuilder.DropTable(
                name: "PlanoLeitura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participante",
                table: "Participante");

            migrationBuilder.RenameTable(
                name: "Participante",
                newName: "Participantes");

            migrationBuilder.RenameIndex(
                name: "IX_Participante_UsuarioId",
                table: "Participantes",
                newName: "IX_Participantes_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participantes",
                table: "Participantes",
                columns: new[] { "GrupoId", "UsuarioId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Participantes_Grupos_GrupoId",
                table: "Participantes",
                column: "GrupoId",
                principalTable: "Grupos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participantes_Usuarios_UsuarioId",
                table: "Participantes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
