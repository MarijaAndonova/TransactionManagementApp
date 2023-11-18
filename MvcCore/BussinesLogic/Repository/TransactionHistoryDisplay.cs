using MvcCore.Models;

namespace MvcCore.BussinesLogic.Repository
{
    public class TransactionHistoryDisplay : ITransactionHistoryDisplay
    {
        private readonly IClientRepository _clientRepository;

        public TransactionHistoryDisplay(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void DisplayTransactionHistory(int clientId)
        {
            List<Transaction> transactions = _clientRepository.GetClientTransactionHistory(clientId);

            // Display the transactions in a user-friendly format (e.g., console output)
            foreach (var transaction in transactions)
            {
                Console.WriteLine($"Transaction ID: {transaction.TransactionId}");
                Console.WriteLine($"Amount: {transaction.Amount}");
                Console.WriteLine($"Date: {transaction.Date}");
                Console.WriteLine($"Type: {transaction.TransactionType}");
                Console.WriteLine("---------------------");
            }
        }
    }
}
