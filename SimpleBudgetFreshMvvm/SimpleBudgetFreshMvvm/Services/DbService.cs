using SimpleBudgetFreshMvvm.Models;
using SimpleBudgetFreshMvvm.PlatformServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBudgetFreshMvvm.Services
{
    public class DbService : IDbService
    {
        IFileService _fileService;
        SQLiteConnection _connection;

        public DbService(IFileService fileService)
        {
            _fileService = fileService;
            _connection = new SQLiteConnection(_fileService.DbPath);
            _connection.CreateTable<MonthItem>();
            _connection.CreateTable<BudgetItem>();
        }

        public List<MonthItem> LoadAllMonths()
        {
            lock(_connection)
            {
                return _connection.Table<MonthItem>().ToList();
            }
        }

        public (MonthItem monthItem, List<BudgetItem> budgetItems) LoadMonth(int month, int year)
        {
            MonthItem monthItem = null;
            List<BudgetItem> budgetItems = null;
            lock (_connection)
            {
                _connection.RunInTransaction(() =>
                {
                    monthItem = _connection.Table<MonthItem>().FirstOrDefault(s => s.Month == month && s.Year == year);
                    budgetItems = _connection.Table<BudgetItem>().Where(s => s.Month == month && s.Year == year).ToList();
                });
            }
            return (monthItem, budgetItems);
        }

        public bool HasMonth(int month, int year)
        {
            MonthItem monthItem = null;
            lock (_connection)
            {
                _connection.RunInTransaction(() =>
                {
                    monthItem = _connection.Table<MonthItem>().FirstOrDefault(s => s.Month == month && s.Year == year);
                });
            }
            return monthItem != null;
        }

        public void SaveMonth(MonthItem monthItem, List<BudgetItem> budgetItems, bool isEditing)
        {
            lock(_connection)
            {
                _connection.RunInTransaction(() => 
                { 
                    if(isEditing)
                    {
                        _connection.InsertOrReplace(monthItem);
                        var items = _connection.Table<BudgetItem>().Where(s => s.Month == monthItem.Month && s.Year == monthItem.Year);
                        foreach (var item in items)
                        {
                            _connection.Delete(item);
                        }
                        _connection.InsertAll(budgetItems);
                    }
                    else
                    {
                        _connection.Insert(monthItem);
                        _connection.InsertAll(budgetItems);
                    }
                });
            }
        }
    }
}
