using MvcCore.BussinesLogic.Repository;
using MvcCore.Models;

namespace MvcCore.BussinesLogic.Services
{
    public class DataService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ITransactionRepository _transactionRepository;

        public DataService(IClientRepository clientRepository, IContractRepository contractRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _contractRepository = contractRepository;
            _transactionRepository = transactionRepository;
        }

        public void SaveClient(Client client)
        {
            _clientRepository.AddClient(client);
            foreach (var contract in client.Contracts)
            {
                _contractRepository.AddContract(contract);
                foreach (var transaction in contract.Transactions)
                {
                    _transactionRepository.AddTransaction(transaction);
                }
            }
        }
    }
}
