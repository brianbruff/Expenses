using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Expenses.Model;

namespace Expenses.Data
{
    public class BasicContentInitializer : IDatabaseInitializer<ExpensesDbContext>
    {
        #region IDatabaseInitializer

        public void InitializeDatabase(ExpensesDbContext context)
        {
            if (context.Database.Exists() && context.Database.CompatibleWithModel(true))
                return;

            if (context.Database.Exists())
                context.Database.Delete();

            context.Database.Create();

            // Put in some startup data
            context.Currencies.Add(new Currency { Code = "GBP" });
            context.Currencies.Add(new Currency { Code = "EUR" });

            

            // Pub in some sample clients
            context.Clients.Add(new Client
            {
                Name = "Datagenic",
                Projects = new List<Project>
                                   {
                                       new Project { Name = "General", Description = "Not related to any specific project" },
                                   }
            });

            // Put in some expense types
            context.ExpenseTypes.Add(new ExpenseType { Name = "Business Entertainment" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Hotel and Subsistence" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Taxi / Travel / Parking" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Staff Entertaining" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Telephone" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Sundry" });
            context.ExpenseTypes.Add(new ExpenseType { Name = "Recoverables" });

            context.SaveChanges();

        }
        #endregion
    }

}