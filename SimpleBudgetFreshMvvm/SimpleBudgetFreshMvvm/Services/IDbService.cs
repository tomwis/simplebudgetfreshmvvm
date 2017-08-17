using System.Collections.Generic;
using SimpleBudgetFreshMvvm.Models;

namespace SimpleBudgetFreshMvvm.Services
{
    public interface IDbService
    {
        List<MonthItem> LoadAllMonths();
        (MonthItem monthItem, List<BudgetItem> budgetItems) LoadMonth(int month, int year);
        void SaveMonth(MonthItem monthItem, List<BudgetItem> budgetItems, bool isEditing);
        bool HasMonth(int month, int year);
    }
}