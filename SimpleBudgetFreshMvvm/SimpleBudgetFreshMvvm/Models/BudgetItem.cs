using SimpleBudgetFreshMvvm.Models.Enums;
using SQLite;
using System.ComponentModel;

namespace SimpleBudgetFreshMvvm.Models
{
    [Table("BudgetItem")]
    public class BudgetItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public BudgetItemType Type { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
