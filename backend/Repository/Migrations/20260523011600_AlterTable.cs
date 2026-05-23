using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AlterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionPlan",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "ActionPlanGeneratedAt",
                table: "Opportunities");

            migrationBuilder.CreateTable(
                name: "OpportunityActionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpportunityId = table.Column<int>(type: "int", nullable: false),
                    AiConfigId = table.Column<int>(type: "int", nullable: false),
                    ActionPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityActionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpportunityActionPlans_AiConfigs_AiConfigId",
                        column: x => x.AiConfigId,
                        principalTable: "AiConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpportunityActionPlans_Opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_OpportunityId",
                table: "Interactions",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActionPlans_AiConfigId",
                table: "OpportunityActionPlans",
                column: "AiConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActionPlans_OpportunityId",
                table: "OpportunityActionPlans",
                column: "OpportunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interactions_Opportunities_OpportunityId",
                table: "Interactions",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interactions_Opportunities_OpportunityId",
                table: "Interactions");

            migrationBuilder.DropTable(
                name: "OpportunityActionPlans");

            migrationBuilder.DropIndex(
                name: "IX_Interactions_OpportunityId",
                table: "Interactions");

            migrationBuilder.AddColumn<string>(
                name: "ActionPlan",
                table: "Opportunities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionPlanGeneratedAt",
                table: "Opportunities",
                type: "datetime2",
                nullable: true);
        }
    }
}
