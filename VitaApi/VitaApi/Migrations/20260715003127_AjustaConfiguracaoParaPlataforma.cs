using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaApi.Migrations
{
    /// <inheritdoc />
    public partial class AjustaConfiguracaoParaPlataforma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "NomeClinica",
                table: "ConfiguracoesClinica");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "ConfiguracoesClinica",
                newName: "WhatsappSuporte");

            migrationBuilder.RenameColumn(
                name: "ResponsavelTecnico",
                table: "ConfiguracoesClinica",
                newName: "EmailSuporte");

            migrationBuilder.RenameColumn(
                name: "CrmResponsavel",
                table: "ConfiguracoesClinica",
                newName: "TelefoneSuporte");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AtualizadoEm",
                table: "ConfiguracoesClinica",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "MensagemPadrao",
                table: "ConfiguracoesClinica",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomePlataforma",
                table: "ConfiguracoesClinica",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MensagemPadrao",
                table: "ConfiguracoesClinica");

            migrationBuilder.DropColumn(
                name: "NomePlataforma",
                table: "ConfiguracoesClinica");

            migrationBuilder.RenameColumn(
                name: "WhatsappSuporte",
                table: "ConfiguracoesClinica",
                newName: "Telefone");

            migrationBuilder.RenameColumn(
                name: "TelefoneSuporte",
                table: "ConfiguracoesClinica",
                newName: "CrmResponsavel");

            migrationBuilder.RenameColumn(
                name: "EmailSuporte",
                table: "ConfiguracoesClinica",
                newName: "ResponsavelTecnico");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AtualizadoEm",
                table: "ConfiguracoesClinica",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "ConfiguracoesClinica",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "ConfiguracoesClinica",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "ConfiguracoesClinica",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ConfiguracoesClinica",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "ConfiguracoesClinica",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ConfiguracoesClinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeClinica",
                table: "ConfiguracoesClinica",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
