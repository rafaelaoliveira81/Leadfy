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
END;
GO