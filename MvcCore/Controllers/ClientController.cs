using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using MvcCore.Data;
using MvcCore.Models;
using System.Security.Policy;
using System.Security.Principal;

namespace MvcCore.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private Client autorizedClient;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index()
        {
              return _context.Clients != null ? 
                          View(await _context.Clients.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details()
        {
            var client = _context.Clients
                .Where(client => client.Name == User.Identity.Name)
                .Include(client => client.Contracts)
                .ToList().FirstOrDefault();

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateContract(string clientId)
        {
            int clientIdParsed = int.Parse(clientId);

            Contrakt contrakt = new()
            {
                ClientId = clientIdParsed,
                Client = _context.Find<Client>(clientIdParsed)
            };

            return View(contrakt);
        }

        [HttpPost]
        public async Task<IActionResult> PostContract(Contrakt contrakt)
        {
            var client = _context.Find<Client>(contrakt.ClientId);
            contrakt.Client = client;
            client.Contracts.Add(contrakt);

            if (ModelState.IsValid)
            {
                _context.Add(contrakt);
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            return View("CreateContract", contrakt);
        }

        [HttpPost]
        public IActionResult CreateTransactions(string contractId)
        {
            int contractIdIdParsed = int.Parse(contractId);

            Transaction transaction = new()
            {
                ContraktId = contractIdIdParsed,
                Contrakt = _context.Find<Contrakt>(contractIdIdParsed)
            };

            int selectedContractId = transaction.Contrakt.Client.SelectedContractId;

            //Simulation of credit and debit transactions for a client that has a contract(bank account)
            if (transaction.TransactionType == BussinesLogic.Enums.TransactionType.Debit)
            {
                transaction.Contrakt.Balance = transaction.Contrakt.Balance - transaction.Amount;
            }
            else
            {
                transaction.Contrakt.Balance = transaction.Contrakt.Balance + transaction.Amount;
            }

            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction(Transaction transaction)
        {
            var contract = _context.Find<Contrakt>(transaction.ContraktId);
            transaction.Contrakt = contract;
            var selectedContractId = transaction.Contrakt.Client.SelectedContractId;

            if (selectedContractId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a contract."); 
                return View("CreateTransactions", transaction); 
            }

            contract.Transactions.Add(transaction);

            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                _context.Update(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            return View("CreateTransactions", transaction);
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
