using Microsoft.EntityFrameworkCore;
using MvcCore.Data;
using MvcCore.Models;

namespace MvcCore.BussinesLogic.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChangesAsync();
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            _context.SaveChangesAsync();
        }

        public DbSet<Client> GetAllClients()
        {
            return _context.Clients;
        }
        public List<Transaction> GetClientTransactionHistory(int clientId)
        {
            return _context.Transactions
                    .Where(t => t.Contrakt.Client.Id == clientId)
                    .ToList();
        }
    }
}
