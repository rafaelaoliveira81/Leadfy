CREATE PROCEDURE sp_CreateAiConfig
    @Title NVARCHAR(150),
    @PromptTemplate NVARCHAR(2000),
    @ApiKeyHash NVARCHAR(512),
    @Model INT
AS
BEGIN
    INSERT INTO AiConfigs (Title, PromptTemplate, ApiKeyHash, Model, IsActive, CreatedAt)
    VALUES (@Title, @PromptTemplate, @ApiKeyHash, @Model, 1, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE PROCEDURE sp_GetAiConfigById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        Title,
        PromptTemplate,
		ApiKeyHash,
		Model,
        IsActive,
        CreatedAt
    FROM AiConfigs
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetAllAiConfigs
    @IsActive BIT = NULL
AS
BEGIN
    SELECT 
        ID,
        Title,
        PromptTemplate,
		ApiKeyHash,
		Model,
        IsActive,
        CreatedAt
    FROM AiConfigs
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_UpdateAiConfig
    @ID INT,
    @Title NVARCHAR(150),
    @PromptTemplate NVARCHAR(2000),
    @ApiKeyHash NVARCHAR(512),
    @Model INT,
    @IsActive BIT
AS
BEGIN
    UPDATE AiConfigs
    SET 
        Title = @Title,
        PromptTemplate = @PromptTemplate,
		ApiKeyHash = @ApiKeyHash,
		Model = @Model,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteAiConfig
    @ID INT
AS
BEGIN
    DELETE FROM AiConfigs
    WHERE ID = @ID;
END;
GO