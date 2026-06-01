-- Function para retornar todos os dados do dashboard em um único resultado
CREATE FUNCTION fn_DashboardAnalytics()
RETURNS TABLE
AS
RETURN
(
    SELECT
        -- Cards principais
        (SELECT TotalLeads FROM vw_TotalLeads) AS TotalLeads,

        (SELECT OpportunitiesInProgress 
         FROM vw_OpportunitiesInProgress) AS OpportunitiesInProgress,

        (SELECT NewWithoutContact 
         FROM vw_NewOpportunitiesWithoutContact) AS NewOpportunitiesWithoutContact,

        (SELECT ForecastRevenue 
         FROM vw_RevenueForecast) AS RevenueForecast,

        (SELECT PipelineValue 
         FROM vw_TotalPipelineValue) AS TotalPipelineValue,

        (SELECT ConversionRate 
         FROM vw_ConversionRate) AS ConversionRate,

        (SELECT RealRevenue 
         FROM vw_RealRevenue) AS RealRevenue,

        -- Lista de valores por stage em JSON
        (
            SELECT
                StageName,
                Quantity,
                TotalValue
            FROM vw_StageAnalytics
            FOR JSON PATH
        ) AS StageAnalytics
);