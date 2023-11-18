using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;

namespace MvcCore.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for Contracts
        public ICollection<Contrakt> Contracts { get; set; }
    }
}
