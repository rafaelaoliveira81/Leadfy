using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddTablePrompt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpportunityActionPlans_AiConfigs_AiConfigId",
                table: "OpportunityActionPlans");

            migrationBuilder.DropTable(
                name: "AiConfigs");

            migrationBuilder.DropIndex(
                name: "IX_OpportunityActionPlans_AiConfigId",
                table: "OpportunityActionPlans");

            migrationBuilder.DropColumn(
                name: "AiConfigId",
                table: "OpportunityActionPlans");

            migrationBuilder.CreateTable(
                name: "Prompts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prompts");

            migrationBuilder.AddColumn<int>(
                name: "AiConfigId",
                table: "OpportunityActionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AiConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Model = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    PromptTemplate = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiConfigs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActionPlans_AiConfigId",
                table: "OpportunityActionPlans",
                column: "AiConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpportunityActionPlans_AiConfigs_AiConfigId",
                table: "OpportunityActionPlans",
                column: "AiConfigId",
                principalTable: "AiConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
