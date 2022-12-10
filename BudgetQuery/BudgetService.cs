using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
            
            var targetBudgets = GetBudgets(start, end);

            if (targetBudgets.Count <= 0)
                return 0;

            decimal result = 0;
            decimal amountPerDay = 0;
            if (targetBudgets.Count >= 2)
            {
                int countDays = 0;
            
                for (int i = 0; i < targetBudgets.Count; i++)
                {
                    if (i == 0)
                    {
                        amountPerDay = targetBudgets[i].Amount / DateTime.DaysInMonth(start.Year, start.Month);
                        countDays =  (DateTime.DaysInMonth(start.Year, start.Month) - start.Day)+1;
                        result += amountPerDay * countDays;
                    }
                    else if (i == targetBudgets.Count - 1)
                    {
                        amountPerDay = targetBudgets[i].Amount / DateTime.DaysInMonth(end.Year, end.Month);
                        countDays = end.Day;
                        result += amountPerDay * countDays;
                    }
                    else
                    {
                        result += targetBudgets[i].Amount;
                    }
                }
            }
            else
            {
                amountPerDay = targetBudgets[0].Amount / DateTime.DaysInMonth(start.Year, start.Month);
                decimal days = (end - start).Days + 1;
                result += amountPerDay * days;
            }
            
            return result;
        }

        private List<Budget> GetBudgets(DateTime start, DateTime end)
        {
            var result = new List<Budget>();
            
            var allBudget = _budgetRepo.GetAll();

            var tempData = start.Add(TimeSpan.Zero);
 
            var keyList = new List<string>();

            while (tempData <= new DateTime(end.Year, end.Month+1, 1).AddDays(-1))
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