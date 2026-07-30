using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAtestadoProfissional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Atestados",
                newName: "Observacoes");

            migrationBuilder.AddColumn<string>(
                name: "Cid",
                table: "Atestados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Atestados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "Atestados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cid",
                table: "Atestados");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Atestados");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "Atestados");

            migrationBuilder.RenameColumn(
                name: "Observacoes",
                table: "Atestados",
                newName: "Descricao");
        }
    }
}
