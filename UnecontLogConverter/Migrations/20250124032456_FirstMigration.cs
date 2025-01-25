using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UnecontLogConverter.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ContentSerialized = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsTransformed",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Fields = table.Column<string>(nullable: true),
                    TransformedContentSerialized = table.Column<string>(nullable: true),
                    LogId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsTransformed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsTransformed_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsTransformed_LogId",
                table: "LogsTransformed",
                column: "LogId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsTransformed");

            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
