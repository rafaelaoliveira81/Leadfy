-- Total de Leads
CREATE VIEW vw_TotalLeads
AS
SELECT 
    COUNT(*) AS TotalLeads
FROM Leads
WHERE IsActive = 1;


-- Oportunidades ativar em andamento
CREATE VIEW vw_OpportunitiesInProgress
AS
SELECT
    COUNT(*) AS OpportunitiesInProgress
FROM Opportunities
WHERE Stage NOT IN (6,7);


-- Oportunidades sem contato
CREATE VIEW vw_NewOpportunitiesWithoutContact
AS
SELECT
    COUNT(*) AS NewWithoutContact
FROM Opportunities
WHERE Stage = 1;


-- Previsão de receita
CREATE VIEW vw_RevenueForecast
AS
SELECT
    SUM(Amount) AS ForecastRevenue
FROM Opportunities
WHERE Stage NOT IN (6,7)
AND ExpectedCloseDate <= DATEADD(DAY,30,GETDATE());


-- Valor total Pipeline
CREATE VIEW vw_TotalPipelineValue
AS
SELECT
    SUM(Amount) AS PipelineValue
FROM Opportunities
WHERE Stage NOT IN (6,7);


-- Taxa de conversão
CREATE VIEW vw_ConversionRate
AS
SELECT
    CAST(
        (
            COUNT(CASE WHEN Stage = 6 THEN 1 END) * 100.0
        ) / NULLIF(COUNT(*),0)
    AS DECIMAL(10,2)) AS ConversionRate
FROM Opportunities;


--Valores por etapa
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
    END AS StageName,

    COUNT(*) AS Quantity,
    SUM(Amount) AS TotalValue

FROM Opportunities
GROUP BY Stage;


-- Receita realizada
CREATE VIEW vw_RealRevenue
AS
SELECT
    SUM(Amount) AS RealRevenue
FROM Opportunities
WHERE Stage = 6;
