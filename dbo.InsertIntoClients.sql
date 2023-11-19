CREATE PROCEDURE InsertIntoClients
    @xmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Clients ([Name], UserName, SelectedContractId)
    SELECT DISTINCT
        ISNULL(NULLIF(@xmlData.value('(/Clients/Client/Name)[1]', 'NVARCHAR(MAX)'), ''), 'AutorizedClient') AS [Name],
        ISNULL(NULLIF(@xmlData.value('(/Clients/Client/UserName)[1]', 'NVARCHAR(MAX)'), ''), 'AutorizedClient') AS UserName,
        0 AS SelectedContractId -- Set initial selected contract
    WHERE NOT EXISTS (
        SELECT 1 FROM Clients c
        WHERE c.[Name] = ISNULL(NULLIF(@xmlData.value('(/Clients/Client/Name)[1]', 'NVARCHAR(MAX)'), ''), 'AutorizedClient')
    );
END
GO