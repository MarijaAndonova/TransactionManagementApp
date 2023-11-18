using MvcCore.Models;

namespace MvcCore.BussinesLogic.Repository
{
    public interface ITransactionRepository
    {
        void AddTransaction(Transaction transaction);
    }
}
