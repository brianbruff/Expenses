using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;

namespace Expenses.Data
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

        #endregion DbSets


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().HasRequired(e => e.Employee).WithMany(t => t.Expenses).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


        
    }
}
