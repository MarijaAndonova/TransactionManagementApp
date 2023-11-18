using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCore.BussinesLogic;
using MvcCore.BussinesLogic.Repository;
using MvcCore.Models;
using System.Diagnostics;

namespace MvcCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientRepository _clientRepository;

        public HomeController(ILogger<HomeController> logger, IClientRepository client)
        {
            _clientRepository = client; 
            _logger = logger;
        }

        //[Authorize]
        public IActionResult Index()
        {
            var clients = _clientRepository.GetAllClients();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Clients()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddClient(Client newClient)
        {
            if (ModelState.IsValid)
            {
                _clientRepository.AddClient(newClient);
                return RedirectToAction("Clients");
            }
            return View(newClient);
        }

        public IActionResult ShowTransactionHistory(int clientId)
        {
            var transactions = _clientRepository.GetClientTransactionHistory(clientId);
            var viewModel = new TransactionHistoryViewModel
            {
                ClientId = clientId,
                Transactions = transactions
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
