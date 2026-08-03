using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanoLeitura2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participante_Grupos_GrupoId",
                table: "Participante");

            migrationBuilder.DropForeignKey(
                name: "FK_Participante_Usuarios_UsuarioId",
                table: "Participante");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
