CREATE PROCEDURE sp_CreateInteraction
    @OpportunityId INT,
	@FromStage INT,
	@ToStage INT,
	@Description NVARCHAR(1000),
    @UserID INT,
	@NextContactDate DATETIME2(7),
	@InteractionDate DATETIME2(7)
AS
BEGIN
    INSERT INTO Interactions (OpportunityId, FromStage, ToStage, Description, InteractionDate, UserID, NextContactDate, CreatedAt)
    VALUES (@OpportunityId, @FromStage, @ToStage, @Description, @InteractionDate, @UserID,@NextContactDate, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE PROCEDURE sp_GetInteractionById
    @ID INT
AS
BEGIN
    SELECT 
        Id,
        OpportunityId,
		FromStage,
		ToStage,
		Description,
		InteractionDate,
        CreatedAt,
		UserId,
		NextContactDate
    FROM Interactions
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllInteractions
AS
BEGIN
    SELECT 
        Id,
        OpportunityId,
		FromStage,
		ToStage,
		Description,
		InteractionDate,
        CreatedAt,
		UserId,
		NextContactDate
    FROM Interactions
    ORDER BY InteractionDate DESC;
END;
GO

CREATE PROCEDURE sp_UpdateInteraction
    @ID INT,
    @OpportunityId INT,
	@FromStage INT,
	@ToStage INT,
	@Description NVARCHAR(1000),
    @UserID INT,
	@NextContactDate DATETIME2(7),
	@InteractionDate DATETIME2(7)
	AS
BEGIN
    UPDATE Interactions
    SET 
        OpportunityId = @OpportunityId,
		FromStage = @FromStage,
		ToStage = @ToStage,
		Description = @Description,
		UserId = @UserID,
		NextContactDate = @NextContactDate,
		InteractionDate = @InteractionDate
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteInteraction
    @ID INT
AS
BEGIN
    DELETE FROM Interactions
    WHERE ID = @ID;
END;
GO