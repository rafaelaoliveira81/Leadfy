CREATE PROCEDURE sp_CreateOpportunityActionPlan
    @OpportunityId INT,
    @ActionPlan NVARCHAR(MAX),
    @GeneratedAt DATETIME2(7)
AS
BEGIN
    INSERT INTO OpportunityActionPlans (OpportunityId, AiConfigId, ActionPlan, GeneratedAt)
    VALUES (@OpportunityId, @AiConfigId, @ActionPlan, @GeneratedAt);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END;
GO

CREATE PROCEDURE sp_GetOpportunityActionPlanById
    @ID INT
AS
BEGIN
    SELECT
        Id,
        OpportunityId,
        AiConfigId,
        ActionPlan,
        GeneratedAt
    FROM OpportunityActionPlans
    WHERE Id = @ID;
END;
GO
