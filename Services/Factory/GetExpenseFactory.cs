using Services.Abstractions.Entities;

namespace Services.Factory
{
    public static class GetExpenseFactory
    {
        public static GetExpense CreateGetExpense(Expense expense, User user)
        {
            return new GetExpense()
            {
                User = $"{user.FirstName} {user.LastName}",
                Date = expense.Date,
                Type = expense.Type,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Comment = expense.Comment,
            };
        }
    }
}