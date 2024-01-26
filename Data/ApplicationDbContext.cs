using BankManagementSystemVersionFinal1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystemVersionFinal1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BankBranch> BankBranches { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<CustomerModel> CustomerModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transfer>()
                .HasOne<Account>(t => t.Sender)
                .WithMany(c => c.Transfers)
                .HasForeignKey(t => t.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transfer>()
                .HasOne<CheckingAccount>(t => t.Receiver)
                .WithMany()
                .HasForeignKey(t => t.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("TypeAccount")
                .HasValue<CheckingAccount>("CheckingAccount")
                .HasValue<SavingAccount>("SavingAccount");

            modelBuilder.Entity<Employee>()
                .HasDiscriminator<string>("Position")
                .HasValue<Manager>("Manager")
                .HasValue<Agent>("Agent");

            base.OnModelCreating(modelBuilder);
        }
    }
}