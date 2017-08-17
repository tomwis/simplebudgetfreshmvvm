using FreshMvvm;
using PropertyChanged;
using SimpleBudgetFreshMvvm.Helpers;
using SimpleBudgetFreshMvvm.Models;
using SimpleBudgetFreshMvvm.Resources;
using SimpleBudgetFreshMvvm.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleBudgetFreshMvvm.ViewModels
{
    public class DashboardViewModel : FreshBasePageModel
    {
        IDbService _dbService;

        public DashboardViewModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            var months = _dbService.LoadAllMonths();
            foreach (var month in months)
            {
                month.MonthName = CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[month.Month - 1];
                if (month.BackgroundColor == null)
                {
                    month.BackgroundColor = ColorHelper.GenerateRandomPleasingColor();
                }
            }
            Months = new List<MonthItem>(months.OrderByDescending(s => s.Year).ThenByDescending(m => m.Month));
            PropertyChanged += DashboardViewModel_PropertyChanged;
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            PropertyChanged -= DashboardViewModel_PropertyChanged;
            base.ViewIsDisappearing(sender, e);
        }

        private async void DashboardViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(SelectedMonth):
                    if (SelectedMonth != null)
                    {
                        await CoreMethods.PushPageModel<MonthEditViewModel>(SelectedMonth);
                        SelectedMonth = null;
                    }
                    break;
            }
        }

        public Command AddMonthCmd => new Command(async () =>
        {
            await CoreMethods.PushPageModel<MonthEditViewModel>();
        });

        public List<MonthItem> Months { get; set; }
        public MonthItem SelectedMonth { get; set; }

        public string Today
        {
            get
            {
                var now = DateTime.Now;
                var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
                return $"{AppResources.DaysToPayout}: {daysInMonth - now.Day + 1}";
            }
        }
    }
}
