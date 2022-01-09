using Repositories.Dtos;
using Services.Entities;
using Shared;

namespace Services.Mappers
{
    public class DbExpenseFromExpenseMapper : IMapper<Expense, DbExpense>
    {
        public DbExpense Map(Expense entity)
        {
            return new DbExpense()
            {
                UserId = entity.UserId,
                Date = entity.Date,
                Type = entity.Type,
                Amount = entity.Amount,
                Currency = entity.Currency,
                Comment = entity.Comment,
            };
        }
    }
}