--DROP PROCEDURE IF EXISTS dbo.InsertIntoTransactions

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

--DROP PROCEDURE IF EXISTS dbo.InsertIntoContrakts

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
 
 --DROP PROCEDURE IF EXISTS dbo.InsertIntoClients


CREATE PROCEDURE InsertIntoClients
    @xmlData XML
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Clients ([Name], UserName, SelectedContractId)
    SELECT DISTINCT
        ISNULL(NULLIF(@xmlData.value('(/Clients/Client/Name)[1]', 'NVARCHAR(MAX)'), ''), 'Unknown') AS [Name],
        ISNULL(NULLIF(@xmlData.value('(/Clients/Client/UserName)[1]', 'NVARCHAR(MAX)'), ''), 'Unknown') AS UserName,
        0 AS SelectedContractId -- Set initial selected contract
    WHERE NOT EXISTS (
        SELECT 1 FROM Clients c
        WHERE c.[Name] = ISNULL(NULLIF(@xmlData.value('(/Clients/Client/Name)[1]', 'NVARCHAR(MAX)'), ''), 'Unknown')
    );
END
GO

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

--Execute this for test

DECLARE @sampleXML XML = '
<Clients>
    <Client ID = "14">
        <Name>Ime7Prezime7</Name>
        <Contracts>
            <Contract>
                <ContractNumber>999999999999</ContractNumber>
                <Balance>100000</Balance>
                <Transactions>
                    <Transaction>
                        <TransactionId>123498</TransactionId>
                        <Amount>2000.00</Amount>
                        <Date>01.11.2011 11:00:00</Date>
                        <TransactionType>Credit</TransactionType>
                    </Transaction>
                    <Transaction>
                        <TransactionId>123499</TransactionId>
                        <Amount>500.00</Amount>
                        <Date>15.10.2023 12:12:00</Date>
                        <TransactionType>Debit</TransactionType>
                    </Transaction>
                </Transactions>
            </Contract>
        </Contracts>
    </Client>
</Clients>';

EXEC InsertTransactionsFromXML @xmlData = @sampleXML;

select * from Transactions
select * from Contrakts
select * from Clients

--Execute this for test

