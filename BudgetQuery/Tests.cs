using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace BudgetQuery
{
    [TestFixture]
    public class BudgetQueryTests
    {
        private DummyBudget _dummy;

        [SetUp]
        public void SetUp()
        {
            _dummy = new DummyBudget();
        }
        
        [Test]
        public void same_month_same_day()
        {
            var budgetService = new BudgetService(_dummy);
            DateTime start = new DateTime(2022,1,1);
            DateTime end =  new DateTime(2022,1,1);;
            Assert.AreEqual(1, budgetService.Query(start, end));
        }
        [Test]
        public void whole_month()
        {
            var budgetService = new BudgetService(_dummy);
            DateTime start = new DateTime(2022,1,1);
            DateTime end =  new DateTime(2022,1,31);;
            Assert.AreEqual(31, budgetService.Query(start, end));
        }
        [Test]
        public void wrong_month_sequnce()
        {
            var budgetService = new BudgetService(_dummy);
            DateTime end = new DateTime(2022,1,1);
            DateTime start =  new DateTime(2022,1,31);;
            Assert.AreEqual(0, budgetService.Query(start, end));
        }
        [Test]
        public void month_no_budget_data()
        {
            var budgetService = new BudgetService(_dummy);
            DateTime start =  new DateTime(1950,1,1);;
            DateTime end = new DateTime(1950,1,2);
            
            Assert.AreEqual(0, budgetService.Query(start, end));
        }
    }

    public class DummyBudget : IBudgetRepo
    {
        public List<Budget> GetAll()
        {
            Budget b = new Budget();
            b.YearMonth = "202201";
            b.Amount = 31;
            return new List<Budget>(){b};
        }
    }
}