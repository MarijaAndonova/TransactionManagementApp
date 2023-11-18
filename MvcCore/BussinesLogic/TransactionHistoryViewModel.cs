using MvcCore.Models;

namespace MvcCore.BussinesLogic
{
    internal class TransactionHistoryViewModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}