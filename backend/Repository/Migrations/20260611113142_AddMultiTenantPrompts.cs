using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddMultiTenantPrompts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Prompts",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Prompts_TenantId",
                table: "Prompts",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prompts_Tenants_TenantId",
                table: "Prompts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetPromptsPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetPromptsPaginado
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
                    FROM Prompts
                    WHERE TenantId = @TenantId
                      AND (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Title,
                        Content,
                        IsActive,
                        CreatedAt
                    FROM Prompts
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
                name: "FK_Prompts_Tenants_TenantId",
                table: "Prompts");

            migrationBuilder.DropIndex(
                name: "IX_Prompts_TenantId",
                table: "Prompts");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetPromptsPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetPromptsPaginado
                    @Status INT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;

                    SELECT COUNT(*) AS TotalRegistros
                    FROM Prompts
                    WHERE (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Title,
                        Content,
                        IsActive,
                        CreatedAt
                    FROM Prompts
                    WHERE (@Status IS NULL OR IsActive = @Status)
                    ORDER BY Id DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Prompts");
        }
    }
}
