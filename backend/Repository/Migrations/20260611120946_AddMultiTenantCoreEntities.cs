using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddMultiTenantCoreEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "OpportunityActionPlans",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Opportunities",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Interactions",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActionPlans_TenantId",
                table: "OpportunityActionPlans",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_TenantId",
                table: "Opportunities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TenantId",
                table: "Leads",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_TenantId",
                table: "Interactions",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interactions_Tenants_TenantId",
                table: "Interactions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Tenants_TenantId",
                table: "Leads",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Tenants_TenantId",
                table: "Opportunities",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpportunityActionPlans_Tenants_TenantId",
                table: "OpportunityActionPlans",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetLeadsPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetLeadsPaginado
                    @TenantId UNIQUEIDENTIFIER,
                    @Status INT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;

                    SELECT COUNT(*) AS TotalRegistros
                    FROM Leads
                    WHERE TenantId = @TenantId
                      AND (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Name,
                        Email,
                        PhoneNumber,
                        IsActive,
                        CreatedAt
                    FROM Leads
                    WHERE TenantId = @TenantId
                      AND (@Status IS NULL OR IsActive = @Status)
                    ORDER BY Id DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interactions_Tenants_TenantId",
                table: "Interactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Tenants_TenantId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Tenants_TenantId",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_OpportunityActionPlans_Tenants_TenantId",
                table: "OpportunityActionPlans");

            migrationBuilder.DropIndex(
                name: "IX_OpportunityActionPlans_TenantId",
                table: "OpportunityActionPlans");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_TenantId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TenantId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Interactions_TenantId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OpportunityActionPlans");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Interactions");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetLeadsPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetLeadsPaginado
                    @Status INT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;

                    SELECT COUNT(*) AS TotalRegistros
                    FROM Leads
                    WHERE (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Name,
                        Email,
                        PhoneNumber,
                        IsActive,
                        CreatedAt
                    FROM Leads
                    WHERE (@Status IS NULL OR IsActive = @Status)
                    ORDER BY Id DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");
        }
    }
}
