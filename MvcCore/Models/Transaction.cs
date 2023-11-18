namespace MvcCore.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ContraktId { get; set; } // Foreign key to Contrakt
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }

        // Navigation property for Contrakt
        public Contrakt Contrakt { get; set; }
    }

}
