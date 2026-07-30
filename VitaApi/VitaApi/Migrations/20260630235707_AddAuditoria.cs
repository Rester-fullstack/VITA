using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atestados_Consultas_ConsultaId",
                table: "Atestados");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosClinicos_Consultas_ConsultaId",
                table: "HistoricosClinicos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosClinicos_Pacientes_PacienteId",
                table: "HistoricosClinicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Receitas_Consultas_ConsultaId",
                table: "Receitas");

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Icone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    UsuarioNome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UsuarioRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsultaId = table.Column<int>(type: "int", nullable: true),
                    PacienteId = table.Column<int>(type: "int", nullable: true),
                    RegistroId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Atestados_Consultas_ConsultaId",
                table: "Atestados",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosClinicos_Consultas_ConsultaId",
                table: "HistoricosClinicos",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosClinicos_Pacientes_PacienteId",
                table: "HistoricosClinicos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Receitas_Consultas_ConsultaId",
                table: "Receitas",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atestados_Consultas_ConsultaId",
                table: "Atestados");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosClinicos_Consultas_ConsultaId",
                table: "HistoricosClinicos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricosClinicos_Pacientes_PacienteId",
                table: "HistoricosClinicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Receitas_Consultas_ConsultaId",
                table: "Receitas");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.AddForeignKey(
                name: "FK_Atestados_Consultas_ConsultaId",
                table: "Atestados",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosClinicos_Consultas_ConsultaId",
                table: "HistoricosClinicos",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricosClinicos_Pacientes_PacienteId",
                table: "HistoricosClinicos",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receitas_Consultas_ConsultaId",
                table: "Receitas",
                column: "ConsultaId",
                principalTable: "Consultas",
                principalColumn: "Id");
        }
    }
}
