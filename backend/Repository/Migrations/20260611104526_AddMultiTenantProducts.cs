using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddMultiTenantProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                table: "Products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId_Name",
                table: "Products",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetProductsPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetProductsPaginado
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
                    FROM Products
                    WHERE TenantId = @TenantId
                      AND (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Name,
                        Description,
                        Price,
                        IsActive,
                        CreatedAt
                    FROM Products
                    WHERE TenantId = @TenantId
                      AND (@Status IS NULL OR IsActive = @Status)
                    ORDER BY Id DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");

             migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetUsersPaginado;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetUsersPaginado
                    @TenantId UNIQUEIDENTIFIER,
                    @IsActive BIT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;
                    
                    SELECT COUNT(*) AS TotalRegistros
                    FROM Users
                    WHERE TenantId = @TenantId
                    AND (@IsActive IS NULL OR IsActive = @IsActive);

                    SELECT
                        ID,
                        Name,
                        Email,
                        PasswordHash,
                        IsActive,
                        CreatedAt
                    FROM Users
                    WHERE TenantId = @TenantId
                    AND (@IsActive IS NULL OR IsActive = @IsActive)
                    ORDER BY Name DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;
                GO"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId_Name",
                table: "Products");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetProductsPaginado;

                CREATE PROCEDURE sp_GetProductsPaginado
                    @Status INT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;

                    SELECT COUNT(*) AS TotalRegistros
                    FROM Products
                    WHERE (@Status IS NULL OR IsActive = @Status);

                    SELECT
                        Id,
                        Name,
                        Description,
                        Price,
                        IsActive,
                        CreatedAt
                    FROM Products
                    WHERE (@Status IS NULL OR IsActive = @Status)
                    ORDER BY Id DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetUsersPaginado;

                CREATE PROCEDURE sp_GetUsersPaginado
                    @TenantId UNIQUEIDENTIFIER,
                    @IsActive BIT = NULL,
                    @Pagina INT = 1,
                    @QuantidadePorPagina INT = 10
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @Pagina < 1
                        SET @Pagina = 1;
                    
                    SELECT COUNT(*) AS TotalRegistros
                    FROM Users
                    WHERE TenantId = @TenantId
                    AND (@IsActive IS NULL OR IsActive = @IsActive);

                    SELECT
                        ID,
                        Name,
                        Email,
                        PasswordHash,
                        IsActive,
                        CreatedAt
                    FROM Users
                    WHERE TenantId = @TenantId
                    AND (@IsActive IS NULL OR IsActive = @IsActive)
                    ORDER BY Name DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;
                GO"
            );

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");
        }
    }
}
