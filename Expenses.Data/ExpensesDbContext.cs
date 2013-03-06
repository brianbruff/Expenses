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
    public class ExpensesDbContext : DbContext, IDatabaseInitializer<ExpensesDbContext>
    {
        public ExpensesDbContext() : base("name=ExpensesConnection")
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


        #region IDatabaseInitializer

        public void InitializeDatabase(ExpensesDbContext context)
        {
            try
            {
               // if (context.Database.Exists() && context.Database.CompatibleWithModel(true))
                 //   return;

                if (context.Database.Exists())
                    context.Database.Delete();

                context.Database.Create();

                // Put in some startup data
                context.Currencies.Add(new Currency { Code = "GBP" });
                context.Currencies.Add(new Currency { Code = "EUR" });

                // Put in some sample employees
                context.Employees.Add(new Employee { BaseCurrency = context.Currencies.Local.First(), Name = "Brian Keating" });

                // Pub in some sample clients
                context.Clients.Add(new Client { Name = "ClientA", 
                    Projects = new List<Project>
                                   {
                                       new Project { Name = "MigrateToV4", Description = "Migrations to version 4 in Japan" },
                                       new Project { Name = "MigrateToV3", Description = "Migrations to version 4 in Denmark" },
                                   }});
                
                context.SaveChanges();
               
            }
            catch (System.Exception exp)
            {
                Trace.TraceError(exp.Message);
                Debugger.Break();

                throw;
            }
        }

        #endregion IDatabaseInitializer
    }
}
