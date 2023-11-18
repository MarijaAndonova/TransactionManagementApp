using Microsoft.EntityFrameworkCore;
using MvcCore.Models;

namespace MvcCore.BussinesLogic.Repository
{
    public interface IClientRepository
    {
        void AddClient(Client client);
        DbSet<Client> GetAllClients();
        List<Transaction> GetClientTransactionHistory(int clientId);
    }
}
