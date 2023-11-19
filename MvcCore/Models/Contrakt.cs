using MvcCore.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCore.Models
{
    public class Contrakt
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Contract number is required")]
        public string ContractNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive number")]
        public decimal Balance { get; set; }
        public int ClientId { get; set; } // Foreign key to Client 

        // Navigation property for Transactions
        public ICollection<Transaction>? Transactions { get; set; }

        // Navigation property for Client
        public Client? Client { get; set; }

        public Contrakt()
        {
            Transactions = new List<Transaction>();
        }
    }

}
