using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Abstractions.Entities;
using Shared;

namespace Services.Contracts
{
    public interface IExpenseService
    {
        Task<Result> CreateExpense(Expense expense);

        Task<Result<IReadOnlyCollection<GetExpense>>> GetExpensesFromUserIdSorted(long userId, string? sortProperty);
    }
}