using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;
using Expenses.Web.Models;

namespace ExpensesTests
{
    public class BasicContentInitializer : IDatabaseInitializer<ExpensesDbContext>
    {
        #region IDatabaseInitializer

        public void InitializeDatabase(ExpensesDbContext context)
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
            context.Employees.Add(new Employee { BaseCurrency = context.Currencies.Local.First(), UserId = "brianbruff" });

            // Pub in some sample clients
            context.Clients.Add(new Client
            {
                Name = "ClientA",
                Projects = new List<Project>
                                   {
                                       new Project { Name = "MigrateToV4", Description = "Migrations to version 4 in Japan" },
                                       new Project { Name = "MigrateToV3", Description = "Migrations to version 4 in Denmark" },
                                   }
            });

            context.SaveChanges();

        }

        #endregion IDatabaseInitializer
    }
}
