using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceitaProfissional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Medicamentos",
                table: "Receitas",
                newName: "Medicamento");

            migrationBuilder.AddColumn<string>(
                name: "Dosagem",
                table: "Receitas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Duracao",
                table: "Receitas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Frequencia",
                table: "Receitas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosagem",
                table: "Receitas");

            migrationBuilder.DropColumn(
                name: "Duracao",
                table: "Receitas");

            migrationBuilder.DropColumn(
                name: "Frequencia",
                table: "Receitas");

            migrationBuilder.RenameColumn(
                name: "Medicamento",
                table: "Receitas",
                newName: "Medicamentos");
        }
    }
}
