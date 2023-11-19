CREATE PROCEDURE InsertIntoTransactions
    @xmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Transactions (ContraktId, TransactionId, Amount, Date, TransactionType)
    SELECT
        ct.Id AS ContraktId,
        TransactionNode.value('(TransactionId)[1]', 'INT') AS TransactionId,
        TransactionNode.value('(Amount)[1]', 'DECIMAL(18, 2)') AS Amount,
        TRY_CONVERT(DATETIME, TransactionNode.value('(Date)[1]', 'NVARCHAR(MAX)'), 104) AS Date,
        CASE TransactionNode.value('(TransactionType)[1]', 'NVARCHAR(50)')
            WHEN 'Credit' THEN 1
            WHEN 'Debit' THEN 2
            ELSE 0 END AS TransactionType
    FROM
        @xmlData.nodes('/Clients/Client/Contracts/Contract/Transactions/Transaction') AS ContractNode(Col)
        CROSS APPLY ContractNode.Col.nodes('../../../Contracts/Contract') AS Contract(ContractNode)
        INNER JOIN Contrakts ct ON ct.ContractNumber = Contract.ContractNode.value('(ContractNumber)[1]', 'NVARCHAR(MAX)')
        INNER JOIN Clients c ON c.Id = ct.ClientId
        CROSS APPLY ContractNode.nodes('Transactions/Transaction') AS TransactionNodes(TransactionNode);
END
GO
