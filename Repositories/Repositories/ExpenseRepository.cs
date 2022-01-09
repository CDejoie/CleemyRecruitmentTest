using System.Collections.Generic;
using System.Linq;
using Repositories.Contracts;
using Repositories.Dtos;

namespace Repositories.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpensesContext _context;

        public ExpenseRepository(ExpensesContext context)
        {
            this._context = context;
        }

        public void CreateExpense(DbExpense dbExpense)
        {
            this._context.Expenses.Add(dbExpense);
            this._context.SaveChanges();
        }

        public IReadOnlyCollection<DbExpense> GetAllFromUserId(long userId)
        {
            return this._context.Expenses.Where(dbExpense => dbExpense.UserId == userId).ToList().AsReadOnly();
        }
    }
}