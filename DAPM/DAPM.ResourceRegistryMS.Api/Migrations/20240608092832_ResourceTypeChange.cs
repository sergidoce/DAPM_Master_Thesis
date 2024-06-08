using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPM.ResourceRegistryMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class ResourceTypeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceTypes_ResourceTypeId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceTypeId",
                table: "Resources");

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "Resources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "Resources");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceTypeId",
                table: "Resources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceTypes_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
