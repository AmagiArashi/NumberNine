using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace BudgetQuery
{
    [TestFixture]
    public class BudgetQueryTests
    {
        private DummyBudget _dummy;
        private BudgetService _budgetService;

        [Test]
        public void same_month_same_day()
        {
            Budget b = new Budget
            {
                YearMonth = "202201",
                Amount = 31
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start = new DateTime(2022,1,1);
            DateTime end =  new DateTime(2022,1,1);
            Assert.AreEqual(1, _budgetService.Query(start, end));
        }
        [Test]
        public void whole_month()
        {
            Budget b = new Budget
            {
                YearMonth = "202201",
                Amount = 31
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start = new DateTime(2022,1,1);
            DateTime end =  new DateTime(2022,1,31);
            Assert.AreEqual(31, _budgetService.Query(start, end));
        }
        [Test]
        public void wrong_month_sequnce()
        {
            Budget b = new Budget
            {
                YearMonth = "202201",
                Amount = 31
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b});
            _budgetService = new BudgetService(_dummy);
            
            DateTime end = new DateTime(2022,1,1);
            DateTime start =  new DateTime(2022,1,31);
            Assert.AreEqual(0, _budgetService.Query(start, end));
        }
        [Test]
        public void month_no_budget_data()
        {
            Budget b = new Budget
            {
                YearMonth = "202201",
                Amount = 31
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start =  new DateTime(1950,1,1);
            DateTime end = new DateTime(1950,1,2);
            
            Assert.AreEqual(0, _budgetService.Query(start, end));
        }
        
        [Test]
        public void month_budget_data_amount_zero()
        {
            Budget b = new Budget
            {
                YearMonth = "202205",
                Amount = 0
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start =  new DateTime(2022,5,1);
            DateTime end = new DateTime(2022,5,31);
            
            Assert.AreEqual(0, _budgetService.Query(start, end));
        }
        [Test]
        public void two_month_with_one_budget_data_amount_zero()
        {
            Budget b1 = new Budget
            {
                YearMonth = "202208",
                Amount = 0
            };
            Budget b2 = new Budget
            {
                YearMonth = "202209",
                Amount = 300
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b1, b2});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start =  new DateTime(2022,8,31);
            DateTime end = new DateTime(2022,9,1);
            
            Assert.AreEqual(10, _budgetService.Query(start, end));
        }
        
        [Test]
        public void cross_two_months()
        {
            Budget b1 = new Budget
            {
                YearMonth = "202201",
                Amount = 31
            };
            
            Budget b2 = new Budget
            {
                YearMonth = "202202",
                Amount = 560
            };
            
            Budget b3 = new Budget
            {
                YearMonth = "202203",
                Amount = 310
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b1,b2,b3});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start =  new DateTime(2022,1,31);
            DateTime end = new DateTime(2022,3,2);
            
            Assert.AreEqual(1+560+20, _budgetService.Query(start, end));
        }
        
        [Test]
        public void cross_month_and_cross_year()
        {
            Budget b1 = new Budget
            {
                YearMonth = "202212",
                Amount = 310
            };
            Budget b2 = new Budget
            {
                YearMonth = "202301",
                Amount = 620
            };
            
            _dummy = new DummyBudget(new List<Budget>(){b1, b2});
            _budgetService = new BudgetService(_dummy);
            
            DateTime start =  new DateTime(2022,12,31);
            DateTime end = new DateTime(2023,1,2);
            
            Assert.AreEqual(10+40, _budgetService.Query(start, end));
        }
    }

    public class DummyBudget : IBudgetRepo
    {
        private List<Budget> _budgets;

        public DummyBudget(List<Budget> budgets)
        {
            _budgets = budgets;
        }

        public List<Budget> GetAll()
        {
            return _budgets;
        }
    }
}