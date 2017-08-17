using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBudgetFreshMvvm.Models
{
    public class MonthDisplay
    {
        public MonthDisplay(string displayName, int number)
        {
            DisplayName = displayName;
            Number = number;
        }

        public string DisplayName { get; set; }
        public int Number { get; set; }
    }
}
