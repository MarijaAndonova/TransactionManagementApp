using MvcCore.Models;
using System.Xml;

namespace MvcCore.BussinesLogic
{
    public class XmlParser
    {
        public List<Client> ParseXml(string xmlFilePath)
        {
            List<Client> clients = new List<Client>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);

                XmlNodeList clientNodes = doc.SelectNodes("//Client");
                if (clientNodes != null)
                {
                    foreach (XmlNode clientNode in clientNodes)
                    {
                        Client client = new Client();
                        client.Id = int.Parse(clientNode.Attributes["ID"].Value);
                        client.Name = clientNode.SelectSingleNode("Name")?.InnerText;

                        List<Contrakt> contracts = new List<Contrakt>();
                        XmlNodeList contractNodes = clientNode.SelectNodes("Contracts/Contract");
                        if (contractNodes != null)
                        {
                            foreach (XmlNode contractNode in contractNodes)
                            {
                                Contrakt contract = new Contrakt();
                                contract.ContractNumber = contractNode.SelectSingleNode("ContractNumber")?.InnerText;
                                contract.Balance = decimal.Parse(contractNode.SelectSingleNode("Balance")?.InnerText);

                                List<Transaction> transactions = new List<Transaction>();
                                XmlNodeList transactionNodes = contractNode.SelectNodes("Transactions/Transaction");
                                if (transactionNodes != null)
                                {
                                    foreach (XmlNode transactionNode in transactionNodes)
                                    {
                                        Transaction transaction = new Transaction();
                                        transaction.TransactionId = int.Parse(transactionNode.SelectSingleNode("TransactionId")?.InnerText);
                                        transaction.Amount = decimal.Parse(transactionNode.SelectSingleNode("Amount")?.InnerText);
                                        transaction.Date = DateTime.ParseExact(
                                            transactionNode.SelectSingleNode("Date")?.InnerText,
                                            "dd.MM.yyyy HH:mm:ss",
                                            null
                                        );
                                        //transaction.TransactionType = transactionNode.SelectSingleNode("TransactionType")?.InnerText;

                                        transactions.Add(transaction);
                                    }
                                }

                                contract.Transactions = transactions;
                                contracts.Add(contract);
                            }
                        }

                        client.Contracts = contracts;
                        clients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing XML: {ex.Message}");
            }

            return clients;
        }
    }

}
