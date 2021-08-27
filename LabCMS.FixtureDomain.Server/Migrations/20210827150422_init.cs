using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LabCMS.FixtureDomain.Server.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    No = table.Column<int>(type: "integer", nullable: false),
                    ProjectShortName = table.Column<string>(type: "text", nullable: false),
                    TestField = table.Column<string>(type: "text", nullable: false),
                    SetIndex = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    StorageInformation = table.Column<string>(type: "text", nullable: false),
                    ShelfNo = table.Column<int>(type: "integer", nullable: false),
                    FloorNo = table.Column<int>(type: "integer", nullable: false),
                    AssetNo = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    ResponsibleTestFields = table.Column<int[]>(type: "integer[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "FixtureEventsInDatabase",
                columns: table => new
                {
                    No = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FixtureNo = table.Column<int>(type: "integer", nullable: false),
                    ContentTypeFullName = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureEventsInDatabase", x => x.No);
                    table.ForeignKey(
                        name: "FK_FixtureEventsInDatabase_Fixtures_FixtureNo",
                        column: x => x.FixtureNo,
                        principalTable: "Fixtures",
                        principalColumn: "No",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixtureEventsInDatabase_FixtureNo",
                table: "FixtureEventsInDatabase",
                column: "FixtureNo");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_ProjectShortName",
                table: "Fixtures",
                column: "ProjectShortName");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_TestField",
                table: "Fixtures",
                column: "TestField");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixtureEventsInDatabase");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
