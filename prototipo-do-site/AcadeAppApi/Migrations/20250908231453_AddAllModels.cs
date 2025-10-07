using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcadeAppApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Usuarios",
                newName: "Telefone");

            migrationBuilder.AddColumn<string>(
                name: "HistoricoAtv",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdiomaPref",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InterfacePref",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Localizacao",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PontosColeta",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RelatorioImpacto",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EstacoesDeColeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Localizacao = table.Column<string>(type: "TEXT", nullable: false),
                    LinkMaps = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstacoesDeColeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QtdLixo = table.Column<float>(type: "REAL", nullable: false),
                    Tempo = table.Column<int>(type: "INTEGER", nullable: false),
                    DataColeta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QtdMaxima = table.Column<float>(type: "REAL", nullable: false),
                    TipoLixo = table.Column<string>(type: "TEXT", nullable: false),
                    Disponibilidade = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trabalhadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    TipoCarro = table.Column<string>(type: "TEXT", nullable: false),
                    TipoLixoRecolhido = table.Column<string>(type: "TEXT", nullable: false),
                    QtdMaxima = table.Column<float>(type: "REAL", nullable: false),
                    Disponibilidade = table.Column<string>(type: "TEXT", nullable: false),
                    HistoricoAtv = table.Column<string>(type: "TEXT", nullable: false),
                    Nota = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabalhadores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstacoesDeColeta");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Trabalhadores");

            migrationBuilder.DropColumn(
                name: "HistoricoAtv",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdiomaPref",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "InterfacePref",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Localizacao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PontosColeta",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RelatorioImpacto",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Usuarios",
                newName: "Username");
        }
    }
}
