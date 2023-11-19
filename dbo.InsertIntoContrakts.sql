CREATE PROCEDURE InsertIntoContrakts
    @xmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Contrakts (ContractNumber, Balance, ClientId)
    SELECT DISTINCT
        ContractNode.Col.value('(../../ContractNumber)[1]', 'NVARCHAR(MAX)') AS ContractNumber,
        @xmlData.value('(/Clients/Client/Contracts/Contract/Balance)[1]', 'DECIMAL(18, 2)') AS Balance,
        @xmlData.value('(//Client/@ID)[1]', 'INT') AS ClientId
    FROM
        @xmlData.nodes('/Clients/Client/Contracts/Contract/Transactions/Transaction') AS ContractNode(Col)
    WHERE NOT EXISTS (
        SELECT 1 FROM Contrakts ct
        WHERE ct.ContractNumber = ContractNode.Col.value('(../../ContractNumber)[1]', 'NVARCHAR(MAX)')
    );
END
GO