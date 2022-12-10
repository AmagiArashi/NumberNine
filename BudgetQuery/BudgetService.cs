using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetQuery
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (end < start) return 0;

            decimal days = (end - start).Days + 1;
            var targetBudgets = GetBudgets(start, end);

            if (targetBudgets.Count <= 0)
                return 0;

            decimal amountPerDay = targetBudgets[0].Amount / 31;
            return amountPerDay * days;
        }

        private List<Budget> GetBudgets(DateTime start, DateTime end)
        {
            var result = new List<Budget>();
            var allBudget = _budgetRepo.GetAll();

            var tempData = start.Add(TimeSpan.Zero);

            var keyList = new List<string>();
            while (tempData <= end)
            {
                var pKey = ConvertPKey(tempData);
                keyList.Add(pKey);

                tempData = tempData.AddMonths(1);
            }

            result = allBudget.Where(x => keyList.Contains(x.YearMonth)).ToList();
            return result;
        }

        private string ConvertPKey(DateTime date)
        {
            return date.ToString("yyyyMM");
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public int Amount;
        public string YearMonth;
    }
}