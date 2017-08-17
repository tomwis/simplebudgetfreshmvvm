using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleBudgetFreshMvvm.Models
{
    [Table("MonthItem")]
    public class MonthItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed(Name = "MonthId", Order = 1, Unique = true)]
        public int Year { get; set; }
        [Indexed(Name = "MonthId", Order = 2, Unique = true)]
        public int Month { get; set; }
        public double LastMonthEarnings { get; set; }
        public double MoneyLeft { get; set; }
        public string MonthName { get; set; }
        public string BackgroundColor { get; set; } = "#00000000";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
