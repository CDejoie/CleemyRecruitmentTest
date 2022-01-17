using Shared;

namespace Services.Errors
{
    public class ExpenseError : Error
    {
        private ExpenseError(string message) : base(message)
        {
        }

        public static ExpenseError DateInTheFuture => new ExpenseError("The date is in the future");

        public static ExpenseError DateIsTooFarAway =>
            new ExpenseError("The date is too far away (more than three months)");

        public static ExpenseError NoComment => new ExpenseError("There is no comment");

        public static ExpenseError NoMatchingCurrency =>
            new ExpenseError("The currency of the expense is different from the user one");

        public static ExpenseError ExpenseAlreadyExist =>
            new ExpenseError("An expense with the same amount and the same date already exist");
    }
}