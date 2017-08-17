using SimpleBudgetFreshMvvm.Models.Enums;
using System.Collections.ObjectModel;

namespace SimpleBudgetFreshMvvm.Models
{
    public class BudgetGroup : ObservableCollection<BudgetItem>
    {
        public string Title { get; set; }
        public BudgetItemType Type { get; set; }
    }
}
