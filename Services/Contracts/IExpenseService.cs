using System.Collections.Generic;
using Services.Entities;
using Shared;

namespace Services.Contracts
{
    public interface IExpenseService
    {
        Result CreateExpense(Expense expense);

        Result<IReadOnlyCollection<GetExpense>> GetExpensesFromUserId(long userId);
    }
}