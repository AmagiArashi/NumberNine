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

        [SetUp]
        public void SetUp()
        {
            

        }
        
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