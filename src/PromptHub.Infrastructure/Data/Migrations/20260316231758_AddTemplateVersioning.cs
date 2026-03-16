using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromptHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateVersioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromptTemplateVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromptTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    DefaultRole = table.Column<int>(type: "integer", nullable: false),
                    MasterInstruction = table.Column<string>(type: "text", nullable: false),
                    RequiredVariables = table.Column<string>(type: "text", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptTemplateVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptTemplateVersions_PromptTemplates_PromptTemplateId",
                        column: x => x.PromptTemplateId,
                        principalTable: "PromptTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplateVersions_PromptTemplateId",
                table: "PromptTemplateVersions",
                column: "PromptTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromptTemplateVersions");
        }
    }
}
