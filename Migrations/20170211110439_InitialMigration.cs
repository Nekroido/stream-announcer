using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Announcer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BroadcastEnd = table.Column<DateTime>(nullable: true),
                    BroadcastStart = table.Column<DateTime>(nullable: false),
                    Channel = table.Column<string>(nullable: true),
                    IsLive = table.Column<bool>(nullable: false),
                    MaxViewers = table.Column<uint>(nullable: false),
                    Media = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Viewers = table.Column<uint>(nullable: false),
                    VkPostId = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataUpdates",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RunDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GiantBombId = table.Column<uint>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Abbreviation = table.Column<string>(nullable: false),
                    GiantBombId = table.Column<uint>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Abbreviation);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    GameId = table.Column<uint>(nullable: false),
                    PlatformAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.GameId, x.PlatformAbbreviation });
                    table.ForeignKey(
                        name: "FK_GamePlatform_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platforms_PlatformAbbreviation",
                        column: x => x.PlatformAbbreviation,
                        principalTable: "Platforms",
                        principalColumn: "Abbreviation",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformAlias",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(nullable: true),
                    PlatformForeignKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformAlias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformAlias_Platforms_PlatformForeignKey",
                        column: x => x.PlatformForeignKey,
                        principalTable: "Platforms",
                        principalColumn: "Abbreviation",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformAbbreviation",
                table: "GamePlatform",
                column: "PlatformAbbreviation");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAlias_PlatformForeignKey",
                table: "PlatformAlias",
                column: "PlatformForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropTable(
                name: "DataUpdates");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "PlatformAlias");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
