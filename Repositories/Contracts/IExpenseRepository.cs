using System.Collections.Generic;
using Repositories.Dtos;

namespace Repositories.Contracts
{
    public interface IExpenseRepository
    {
        void CreateExpense(DbExpense dbExpense);

        IReadOnlyCollection<DbExpense> GetAllFromUserId(long userId);
    }
}