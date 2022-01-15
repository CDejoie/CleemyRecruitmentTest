using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Abstractions.Entities;

namespace Services.Abstractions.Repositories.Contracts
{
    public interface IExpenseRepository
    {
        Task CreateExpense(Expense dbExpense);

        Task<IReadOnlyCollection<Expense>> GetAllFromUserIdSorted(long userId, string? sortProperty);

        Task<bool> ExpenseAlreadyExist(Expense expense);
    }
}