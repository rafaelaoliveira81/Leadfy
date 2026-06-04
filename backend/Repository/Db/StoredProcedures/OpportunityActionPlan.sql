CREATE PROCEDURE sp_CreateOpportunityActionPlan
    @OpportunityId INT,
    @Message NVARCHAR(MAX),
    @ActionPlan NVARCHAR(MAX),
    @GeneratedAt DATETIME2(7)
AS
BEGIN
    INSERT INTO OpportunityActionPlans (OpportunityId, Message, ActionPlan, GeneratedAt)
    VALUES (@OpportunityId, @Message, @ActionPlan, @GeneratedAt);

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
        Message,
        ActionPlan,
        GeneratedAt
    FROM OpportunityActionPlans
    WHERE Id = @ID;
END;
GO
