using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcCore.Models;
using System.Diagnostics.Contracts;
using System.Transactions;

namespace BusinessLogicLibrary
{
    public class TransactionDbContext : DbContext
    {
        #region Fields and properties
        private readonly IConfiguration _configuration;

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contrakt> Contracts { get; set; }
        public DbSet<MvcCore.Models.Transaction> Transactions { get; set; }
        #endregion Fields and properties

        #region Constructors
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        #endregion Constructors

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Contrakt>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Contrakt)
                .HasForeignKey(t => t.ContraktId);
        }
        #endregion Methods
    }
}
