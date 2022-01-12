using Repositories.Dtos;
using Services.Entities;
using Shared;

namespace Services.Mappers
{
    public class ExpenseFromDbExpenseMapper : IMapper<DbExpense, Expense>
    {
        public Expense Map(DbExpense dbEntity)
        {
            return new Expense()
            {
                UserId = dbEntity.UserId,
                Date = dbEntity.Date,
                Type = dbEntity.Type,
                Amount = dbEntity.Amount,
                Currency = dbEntity.Currency,
                Comment = dbEntity.Comment,
            };
        }
    }
}