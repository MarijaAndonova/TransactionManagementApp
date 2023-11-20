CREATE PROCEDURE GetClientTransactions
    @ClientId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT T.*
    FROM Transactions T
    INNER JOIN Contrakts C ON T.ContraktId = C.Id
    INNER JOIN Clients Cl ON C.ClientId = Cl.Id
    WHERE Cl.Id = @ClientId;
END
GO