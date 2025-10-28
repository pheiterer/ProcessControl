using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProcessControl.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroProcesso = table.Column<string>(type: "text", nullable: false),
                    Autor = table.Column<string>(type: "text", nullable: false),
                    Reu = table.Column<string>(type: "text", nullable: false),
                    DataAjuizamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoricosProcesso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessoId = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosProcesso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosProcesso_Processos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosProcesso_ProcessoId",
                table: "HistoricosProcesso",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processos_NumeroProcesso",
                table: "Processos",
                column: "NumeroProcesso",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricosProcesso");

            migrationBuilder.DropTable(
                name: "Processos");
        }
    }
}
