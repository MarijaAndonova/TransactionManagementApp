CREATE PROCEDURE InsertTransactionsFromXML
    @xmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    --ToDo Check if client and contract exist 
    EXEC InsertIntoClients @xmlData;
    EXEC InsertIntoContrakts @xmlData;
    EXEC InsertIntoTransactions @xmlData;

END
GO