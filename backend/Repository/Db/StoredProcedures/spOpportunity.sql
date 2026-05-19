CREATE PROCEDURE sp_CreateOpportunity
    @ActionPlan NVARCHAR(MAX) = NULL,
    @ActionPlanGeneratedAt DATETIME = NULL,
    @LeadId INT,
    @OwnerId INT = NULL,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME,
    @SortOrder INT
AS
BEGIN
    INSERT INTO Opportunities (
        ActionPlan,
        ActionPlanGeneratedAt,
        LeadId,
        OwnerId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    )
    VALUES (
        @ActionPlan,
        @ActionPlanGeneratedAt,
        @LeadId,
        @OwnerId,
        @ProductId,
        @Stage,
        @Amount,
        @ExpectedCloseDate,
        GETUTCDATE(),
        1,
        @SortOrder
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ID;
END;
GO

CREATE PROCEDURE sp_GetOpportunityById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        ActionPlan,
        ActionPlanGeneratedAt,
        LeadId,
        OwnerId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    FROM Opportunities
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllOpportunities
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        ActionPlan,
        ActionPlanGeneratedAt,
        LeadId,
        OwnerId,
        ProductId,
        Stage,
        Amount,
        ExpectedCloseDate,
        CreatedAt,
        IsActive,
        SortOrder
    FROM Opportunities
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_UpdateOpportunity
    @ID INT,
    @ActionPlan NVARCHAR(MAX) = NULL,
    @ActionPlanGeneratedAt DATETIME = NULL,
    @LeadId INT,
    @OwnerId INT = NULL,
    @ProductId INT,
    @Stage INT,
    @Amount DECIMAL(18, 2),
    @ExpectedCloseDate DATETIME,
    @IsActive BIT,
    @SortOrder INT
AS
BEGIN
    UPDATE Opportunities
    SET 
        ActionPlan = @ActionPlan,
        ActionPlanGeneratedAt = @ActionPlanGeneratedAt,
        LeadId = @LeadId,
        OwnerId = @OwnerId,
        ProductId = @ProductId,
        Stage = @Stage,
        Amount = @Amount,
        ExpectedCloseDate = @ExpectedCloseDate,
        IsActive = @IsActive,
        SortOrder = @SortOrder
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteOpportunity
    @ID INT
AS
BEGIN
    DELETE FROM Opportunities
    WHERE ID = @ID;
END;
GO