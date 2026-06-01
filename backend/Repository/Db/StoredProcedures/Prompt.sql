CREATE PROCEDURE sp_CreatePrompt
    @Title NVARCHAR(150),
    @Content NVARCHAR(1000),
    @IsActive BIT
AS
BEGIN
    INSERT INTO Prompts (Title, Content, IsActive, CreatedAt)
    VALUES (@Title, @Content, @IsActive, GETDATE());

    SELECT SCOPE_IDENTITY() AS ID;
END;
GO

CREATE PROCEDURE sp_GetPromptById
    @ID INT
AS
BEGIN
    SELECT 
        ID,
        Title,
        Content,
        IsActive,
        CreatedAt
    FROM Prompts
    WHERE ID = @ID;
END;
GO

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
END;
GO


CREATE PROCEDURE sp_UpdatePrompt
    @ID INT,
    @Title NVARCHAR(150),
    @Content NVARCHAR(1000),
    @IsActive BIT
AS
BEGIN
    UPDATE Prompts
    SET 
        Title = @Title,
        Content = @Content,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeletePrompt
    @ID INT
AS
BEGIN
    DELETE FROM Prompts
    WHERE ID = @ID;
END;
GO