CREATE PROCEDURE sp_GetLeadsPaginado
    @Status INT = NULL,
    @Pagina INT = 1,
    @QuantidadePorPagina INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @Pagina < 1
        SET @Pagina = 1;

    SELECT COUNT(*) AS TotalRegistros
    FROM Leads
    WHERE (@Status IS NULL OR IsActive = @Status);

    SELECT
        Id,
        Name,
        Email,
        PhoneNumber,
        IsActive,
        CreatedAt
    FROM Leads
    WHERE (@Status IS NULL OR IsActive = @Status)
    ORDER BY Id DESC
    OFFSET (@Pagina - 1) * @QuantidadePorPagina ROWS
    FETCH NEXT @QuantidadePorPagina ROWS ONLY;
END;
GO