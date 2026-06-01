using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class MakeOpportunityProductIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Opportunities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_CreateOpportunity
    @LeadId INT,
    @ProductId INT = NULL,
    @UserId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME = NULL,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (
        LeadId,
        UserId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        SortOrder
    )
    VALUES (
        @LeadId,
        @UserId,
        @ProductId,
        @Stage,
        @Amount,
        @ExpectedCloseDate,
        GETUTCDATE(),
        @SortOrder
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_UpdateOpportunity
    @ID INT,
    @LeadId INT,
    @ProductId INT = NULL,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME = NULL,
    @SortOrder INT
AS
BEGIN
    UPDATE Opportunities
    SET
        LeadId = @LeadId,
        ProductId = @ProductId,
        Stage = @Stage,
        Amount = @Amount,
        ExpectedCloseDate = @ExpectedCloseDate,
        SortOrder = @SortOrder
    WHERE ID = @ID;
END
');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Opportunities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_CreateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CreateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_CreateOpportunity
    @LeadId INT,
    @ProductId INT,
    @UserId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME = NULL,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (
        LeadId,
        UserId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        SortOrder
    )
    VALUES (
        @LeadId,
        @UserId,
        @ProductId,
        @Stage,
        @Amount,
        @ExpectedCloseDate,
        GETUTCDATE(),
        @SortOrder
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END
');");

            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[sp_UpdateOpportunity]', N'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_UpdateOpportunity];
EXEC(N'
CREATE PROCEDURE sp_UpdateOpportunity
    @ID INT,
    @LeadId INT,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME = NULL,
    @SortOrder INT
AS
BEGIN
    UPDATE Opportunities
    SET
        LeadId = @LeadId,
        ProductId = @ProductId,
        Stage = @Stage,
        Amount = @Amount,
        ExpectedCloseDate = @ExpectedCloseDate,
        SortOrder = @SortOrder
    WHERE ID = @ID;
END
');");
        }
    }
}
