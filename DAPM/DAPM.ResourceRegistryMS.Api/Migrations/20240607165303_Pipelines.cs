using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPM.ResourceRegistryMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class Pipelines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pipelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PeerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pipelines", x => new { x.PeerId, x.RepositoryId, x.Id });
                    table.ForeignKey(
                        name: "FK_Pipelines_Peers_PeerId",
                        column: x => x.PeerId,
                        principalTable: "Peers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pipelines_Repositories_PeerId_RepositoryId",
                        columns: x => new { x.PeerId, x.RepositoryId },
                        principalTable: "Repositories",
                        principalColumns: new[] { "PeerId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pipelines");
        }
    }
}
