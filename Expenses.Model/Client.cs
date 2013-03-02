using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
    }
}
