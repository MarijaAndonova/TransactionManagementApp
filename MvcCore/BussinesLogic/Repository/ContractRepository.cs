using MvcCore.Data;
using MvcCore.Models;

namespace MvcCore.BussinesLogic.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddContract(Contrakt contract)
        {
            _context.Contrakts.Add(contract);
            _context.SaveChanges();
        }
    }
}
