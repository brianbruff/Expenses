using System.Data.Entity;
using Expenses.Model;

namespace Expenses.Web.Models
{
    public class ExpensesDbContext : DbContext
    {
        public ExpensesDbContext() : base("name=DefaultConnection")
        {
        }

        #region DbSets

        public DbSet<Client> Clients { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        #endregion DbSets


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().HasRequired(e => e.Employee).WithMany(t => t.Expenses).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


        
    }
}
