using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prompts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Opportunities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ExpectedCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opportunities_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Opportunities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Opportunities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpportunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromStage = table.Column<int>(type: "int", nullable: true),
                    ToStage = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextContactDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interactions_Opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpportunityActionPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpportunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityActionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpportunityActionPlans_Opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_OpportunityId",
                table: "Interactions",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_UserId",
                table: "Interactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_LeadId",
                table: "Opportunities",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_ProductId",
                table: "Opportunities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_UserId",
                table: "Opportunities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpportunityActionPlans_OpportunityId",
                table: "OpportunityActionPlans",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

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

            migrationBuilder.Sql(@"
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

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_GetUsersPaginado
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
                    WHERE (@IsActive IS NULL OR IsActive = @IsActive);

                    SELECT
                        ID,
                        Name,
                        Email,
                        PasswordHash,
                        IsActive,
                        CreatedAt
                    FROM Users
                    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
                    ORDER BY Name DESC
                    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
                    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
                END;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_TotalLeads
                AS
                SELECT
                    COUNT(*) AS TotalLeads
                FROM Leads
                WHERE IsActive = 1;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_OpportunitiesInProgress
                AS
                SELECT
                    COUNT(*) AS OpportunitiesInProgress
                FROM Opportunities
                WHERE Stage NOT IN (1,6,7);");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_NewOpportunitiesWithoutContact
                AS
                SELECT
                    COUNT(*) AS NewWithoutContact
                FROM Opportunities
                WHERE Stage = 1;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_RevenueForecast
                AS
                SELECT
                    ISNULL(SUM(Amount),0) AS ForecastRevenue
                FROM Opportunities
                WHERE Stage NOT IN (6,7)
                AND ExpectedCloseDate <= DATEADD(DAY, 30, GETDATE());");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_TotalPipelineValue
                AS
                SELECT
                    ISNULL(SUM(Amount),0) AS PipelineValue
                FROM Opportunities
                WHERE Stage NOT IN (6,7);");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_ConversionRate
                AS
                SELECT
                    CAST(
                        (
                            COUNT(CASE WHEN Stage = 6 THEN 1 END) * 100.0
                        ) / NULLIF(COUNT(*),0)
                    AS DECIMAL(10,2)) AS ConversionRate
                FROM Opportunities;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_StageAnalytics
                AS
                SELECT
                    CASE Stage
                        WHEN 1 THEN 'Novo Lead'
                        WHEN 2 THEN 'Em Contato'
                        WHEN 3 THEN 'Qualificado'
                        WHEN 4 THEN 'Proposta Enviada'
                        WHEN 5 THEN 'Negociação'
                        WHEN 6 THEN 'Ganho'
                        WHEN 7 THEN 'Perdido'
                        ELSE 'Não Definido'
                    END AS StageName,

                    COUNT(*) AS Quantity,

                    ISNULL(SUM(Amount),0) AS TotalValue

                FROM Opportunities
                GROUP BY Stage;");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_RealRevenue
                AS
                SELECT
                    ISNULL(SUM(Amount),0) AS RealRevenue
                FROM Opportunities
                WHERE Stage = 6;");

            migrationBuilder.Sql(@"
                CREATE FUNCTION fn_DashboardAnalytics()
                RETURNS TABLE
                AS
                RETURN
                (
                    SELECT
                        (SELECT TotalLeads FROM vw_TotalLeads) AS TotalLeads,
                        (SELECT OpportunitiesInProgress FROM vw_OpportunitiesInProgress) AS OpportunitiesInProgress,
                        (SELECT NewWithoutContact FROM vw_NewOpportunitiesWithoutContact) AS NewOpportunitiesWithoutContact,
                        (SELECT ForecastRevenue FROM vw_RevenueForecast) AS RevenueForecast,
                        (SELECT PipelineValue FROM vw_TotalPipelineValue) AS TotalPipelineValue,
                        (SELECT ConversionRate FROM vw_ConversionRate) AS ConversionRate,
                        (SELECT RealRevenue FROM vw_RealRevenue) AS RealRevenue,
                        (
                            SELECT
                                StageName,
                                Quantity,
                                TotalValue
                            FROM vw_StageAnalytics
                            FOR JSON PATH
                        ) AS StageAnalytics
                );");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_DashboardAnalytics;");

            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RealRevenue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_StageAnalytics;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ConversionRate;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalPipelineValue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RevenueForecast;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_NewOpportunitiesWithoutContact;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_OpportunitiesInProgress;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalLeads;");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetUsersPaginado;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetPromptsPaginado;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetProductsPaginado;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetLeadsPaginado;");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropTable(
                name: "OpportunityActionPlans");

            migrationBuilder.DropTable(
                name: "Prompts");

            migrationBuilder.DropTable(
                name: "Opportunities");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
