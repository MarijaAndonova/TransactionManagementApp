using System.ComponentModel.DataAnnotations;
using static MvcCore.BussinesLogic.Enums;

namespace MvcCore.Models
{
    public class Transaction : IValidatableObject
    {
        public int Id { get; set; }
        public int ContraktId { get; set; } // Foreign key to Contrakt
        public int TransactionId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Transaction type is required")]
        public TransactionType TransactionType { get; set; }

        // Navigation property for Contrakt
        public Contrakt Contrakt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount < 0)
            {
                yield return new ValidationResult("Amount cannot be negative", new[] { nameof(Amount) });
            }

            if (Date > DateTime.Now)
            {
                yield return new ValidationResult("Date cannot be in the future", new[] { nameof(Date) });
            }
        }
    }

}
