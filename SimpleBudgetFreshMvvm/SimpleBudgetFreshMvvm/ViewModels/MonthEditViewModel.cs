using FreshMvvm;
using SimpleBudgetFreshMvvm.Helpers;
using SimpleBudgetFreshMvvm.Models;
using SimpleBudgetFreshMvvm.Models.Enums;
using SimpleBudgetFreshMvvm.Resources;
using SimpleBudgetFreshMvvm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleBudgetFreshMvvm.ViewModels
{
    public class MonthEditViewModel : FreshBasePageModel
    {
        IDbService _dbService;
        bool _isEditing;

        public MonthEditViewModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            FillData();
            if (initData is MonthItem monthItem)
            {
                _isEditing = true;
                MonthItem = monthItem;
                var (_, budgetItems) = _dbService.LoadMonth(monthItem.Month, monthItem.Year);

                foreach (var group in BudgetItems)
                {
                    foreach (var item in budgetItems.Where(s => s.Type == group.Type))
                    {
                        group.Add(item);
                    }
                }
            }
            else
            {
                _isEditing = false;
                var now = DateTime.Now;
                MonthItem = new MonthItem
                {
                    Month = now.Month,
                    Year = now.Year
                };
            }

            SelectedMonth = Months.FirstOrDefault(s => s.Number == MonthItem.Month);
            PropertyChanged += MonthEditViewModel_PropertyChanged;
            MonthItem.PropertyChanged += MonthItem_PropertyChanged;
            foreach (var group in BudgetItems)
            {
                foreach (var item in group)
                {
                    item.PropertyChanged += BudgetItem_PropertyChanged;
                }
            }
        }

        void FillData()
        {
            BudgetItems = new ObservableCollection<BudgetGroup>
            {
                new BudgetGroup { Title = AppResources.RecurringItemsGroupLabel, Type = BudgetItemType.Recurring },
                new BudgetGroup { Title = AppResources.OneTimeItemsGroupLabel, Type = BudgetItemType.OneTime }
            };
            int yearsToShow = 10;
            Years = Enumerable.Range(DateTime.Now.Year - yearsToShow, yearsToShow + 1).ToList();
            Months = new List<MonthDisplay>(DateTimeFormatInfo.CurrentInfo.MonthNames.Where(s => !string.IsNullOrEmpty(s)).Select((s, i) => new MonthDisplay(s, i + 1)));
        }

        private void BudgetItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BudgetItem.Amount):
                    CalculateMoneyLeft();
                    break;
            }
        }

        private void MonthItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MonthItem.LastMonthEarnings):
                    CalculateMoneyLeft();
                    break;
            }
        }

        void CalculateMoneyLeft()
        {
            var moneyLeft = MonthItem.LastMonthEarnings;
            foreach (var group in BudgetItems)
            {
                foreach (var item in group)
                {
                    moneyLeft -= item.Amount;
                }
            }
            MonthItem.MoneyLeft = moneyLeft;
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            PropertyChanged -= MonthEditViewModel_PropertyChanged;
            MonthItem.PropertyChanged -= MonthItem_PropertyChanged;
            foreach (var group in BudgetItems)
            {
                foreach (var item in group)
                {
                    item.PropertyChanged -= BudgetItem_PropertyChanged;
                }
            }
            base.ViewIsDisappearing(sender, e);
        }

        private void MonthEditViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedMonth):
                    if (SelectedMonth != null)
                    {
                        MonthItem.Month = SelectedMonth.Number;
                    }
                    break;
            }
        }

        public Command<BudgetItemType> AddExpeseCmd => new Command<BudgetItemType>(t => 
        {
            var newItem = new BudgetItem { Type = t };
            newItem.PropertyChanged += BudgetItem_PropertyChanged;
            BudgetItems.First(s => s.Type == t).Add(newItem);
        });

        public Command<BudgetItem> DeleteExpeseCmd => new Command<BudgetItem>(b => 
        {
            var group = BudgetItems.First(s => s.Type == b.Type);
            b.PropertyChanged -= BudgetItem_PropertyChanged;
            group.Remove(b);
            CalculateMoneyLeft();
        });

        public Command SaveCmd => new Command(async () => 
        {
            // save
            var items = new List<BudgetItem>(BudgetItems[0]);
            items.AddRange(BudgetItems[1]);

            foreach (var item in items)
            {
                item.Month = MonthItem.Month;
                item.Year = MonthItem.Year;
            }

            if (!_isEditing && _dbService.HasMonth(MonthItem.Month, MonthItem.Year))
            {
                await CoreMethods.DisplayAlert("", AppResources.MothAlreadyExistsMessage, AppResources.Ok);
            }
            else
            {
                MonthItem.BackgroundColor = ColorHelper.GenerateRandomPleasingColor();
                _dbService.SaveMonth(MonthItem, items, _isEditing);
                await CoreMethods.PopPageModel();
            }
        });

        public MonthItem MonthItem { get; set; }
        public ObservableCollection<BudgetGroup> BudgetItems { get; set; }
        public List<int> Years { get; set; }
        public List<MonthDisplay> Months { get; set; }
        public MonthDisplay SelectedMonth { get; set; }
    }
}
