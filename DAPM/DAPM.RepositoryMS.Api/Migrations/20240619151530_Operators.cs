using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPM.RepositoryMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class Operators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SourceCodeFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    DockerfileFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operators_Files_DockerfileFileId",
                        column: x => x.DockerfileFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operators_Files_SourceCodeFileId",
                        column: x => x.SourceCodeFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operators_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operators_DockerfileFileId",
                table: "Operators",
                column: "DockerfileFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_RepositoryId",
                table: "Operators",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_SourceCodeFileId",
                table: "Operators",
                column: "SourceCodeFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operators");
        }
    }
}
