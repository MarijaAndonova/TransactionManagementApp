namespace MvcCore.Models
{
    public class Contrakt
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public decimal Balance { get; set; }
        public string ClientId { get; set; } // Foreign key to Client coming from IdentityUser

        // Navigation property for Transactions
        public ICollection<Transaction> Transactions { get; set; }

        // Navigation property for Client
        public Client Client { get; set; }
    }

}
