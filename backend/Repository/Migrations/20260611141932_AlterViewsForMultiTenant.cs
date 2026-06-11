using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AlterViewsForMultiTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_DashboardSummary;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_DashboardAnalytics;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RealRevenue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_StageAnalytics;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ConversionRate;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalPipelineValue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RevenueForecast;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_NewOpportunitiesWithoutContact;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_OpportunitiesInProgress;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalLeads;");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DashboardSummary
                    @TenantId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @TenantId IS NULL
                    BEGIN
                        THROW 50001, 'TenantId e obrigatorio.', 1;
                    END;

                    DECLARE @Now DATETIME2(0) = SYSDATETIME();
                    DECLARE @ForecastLimitDate DATETIME2(0) = DATEADD(DAY, 30, @Now);

                    ;WITH OpportunityAgg AS
                    (
                        SELECT
                            COUNT(1) AS TotalOpportunities,
                            SUM(CASE WHEN Stage NOT IN (1,6,7) THEN 1 ELSE 0 END) AS OpportunitiesInProgress,
                            SUM(CASE WHEN Stage = 1 THEN 1 ELSE 0 END) AS NewWithoutContact,
                            SUM(CASE WHEN Stage NOT IN (6,7) THEN ISNULL(Amount, 0) ELSE 0 END) AS PipelineValue,
                            SUM(CASE WHEN Stage = 6 THEN ISNULL(Amount, 0) ELSE 0 END) AS RealRevenue,
                            SUM(CASE
                                    WHEN Stage NOT IN (6,7)
                                     AND ExpectedCloseDate <= @ForecastLimitDate
                                    THEN ISNULL(Amount, 0)
                                    ELSE 0
                                END) AS ForecastRevenue,
                            SUM(CASE WHEN Stage = 6 THEN 1 ELSE 0 END) AS WonOpportunities
                        FROM Opportunities
                        WHERE TenantId = @TenantId
                    )
                    SELECT
                        (
                            SELECT COUNT(*)
                            FROM Leads
                            WHERE TenantId = @TenantId
                              AND IsActive = 1
                        ) AS TotalLeads,
                        ISNULL(oa.OpportunitiesInProgress, 0) AS OpportunitiesInProgress,
                        ISNULL(oa.NewWithoutContact, 0) AS NewWithoutContact,
                        ISNULL(oa.ForecastRevenue, 0) AS ForecastRevenue,
                        ISNULL(oa.PipelineValue, 0) AS PipelineValue,
                        ISNULL(oa.RealRevenue, 0) AS RealRevenue,
                        CAST(
                            ISNULL(oa.WonOpportunities, 0) * 100.0
                            / NULLIF(oa.TotalOpportunities, 0)
                            AS DECIMAL(10,2)
                        ) AS ConversionRate
                    FROM OpportunityAgg oa;
                END;
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_DashboardStageAnalytics;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DashboardStageAnalytics
                    @TenantId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @TenantId IS NULL
                    BEGIN
                        THROW 50001, 'TenantId e obrigatorio.', 1;
                    END;

                    SELECT
                        Stage,
                        CASE Stage
                            WHEN 1 THEN 'Novo Lead'
                            WHEN 2 THEN 'Em Contato'
                            WHEN 3 THEN 'Qualificado'
                            WHEN 4 THEN 'Proposta Enviada'
                            WHEN 5 THEN 'Negociacao'
                            WHEN 6 THEN 'Ganho'
                            WHEN 7 THEN 'Perdido'
                            ELSE 'Nao Definido'
                        END AS StageName,
                        COUNT(*) AS Quantity,
                        ISNULL(SUM(Amount), 0) AS TotalValue
                    FROM Opportunities
                    WHERE TenantId = @TenantId
                    GROUP BY Stage
                    ORDER BY Stage;
                END;
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_DashboardSummary;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_DashboardAnalytics;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RealRevenue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_StageAnalytics;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_ConversionRate;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalPipelineValue;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_RevenueForecast;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_NewOpportunitiesWithoutContact;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_OpportunitiesInProgress;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_TotalLeads;");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DashboardSummary
                    @TenantId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        (
                            SELECT COUNT(*)
                            FROM Leads
                            WHERE TenantId = @TenantId
                              AND IsActive = 1
                        ) AS TotalLeads,
                        (
                            SELECT COUNT(*)
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                              AND Stage NOT IN (1,6,7)
                        ) AS OpportunitiesInProgress,
                        (
                            SELECT COUNT(*)
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                              AND Stage = 1
                        ) AS NewWithoutContact,
                        (
                            SELECT ISNULL(SUM(Amount), 0)
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                              AND Stage NOT IN (6,7)
                              AND ExpectedCloseDate <= DATEADD(DAY, 30, GETDATE())
                        ) AS ForecastRevenue,
                        (
                            SELECT ISNULL(SUM(Amount), 0)
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                              AND Stage NOT IN (6,7)
                        ) AS PipelineValue,
                        (
                            SELECT ISNULL(SUM(Amount), 0)
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                              AND Stage = 6
                        ) AS RealRevenue,
                        (
                            SELECT CAST(
                                (
                                    COUNT(CASE WHEN Stage = 6 THEN 1 END) * 100.0
                                ) / NULLIF(COUNT(*), 0)
                                AS DECIMAL(10,2)
                            )
                            FROM Opportunities
                            WHERE TenantId = @TenantId
                        ) AS ConversionRate;
                END;
            ");

            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_DashboardStageAnalytics;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DashboardStageAnalytics
                    @TenantId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        Stage,
                        CASE Stage
                            WHEN 1 THEN 'Novo Lead'
                            WHEN 2 THEN 'Em Contato'
                            WHEN 3 THEN 'Qualificado'
                            WHEN 4 THEN 'Proposta Enviada'
                            WHEN 5 THEN 'Negociacao'
                            WHEN 6 THEN 'Ganho'
                            WHEN 7 THEN 'Perdido'
                            ELSE 'Nao Definido'
                        END AS StageName,
                        COUNT(*) AS Quantity,
                        ISNULL(SUM(Amount), 0) AS TotalValue
                    FROM Opportunities
                    WHERE TenantId = @TenantId
                    GROUP BY Stage
                    ORDER BY Stage;
                END;
            ");
        }
    }
}