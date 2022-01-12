namespace Services.Response
{
    using Shared;

    public class ExpenseResponse : Response
    {
        private ExpenseResponse(string value) : base(value)
        {
        }

        public static ExpenseResponse DateInTheFuture => new ExpenseResponse("The date is in the future");

        public static ExpenseResponse DateIsTooFarAway =>
            new ExpenseResponse("The date is too far away (more than three months)");

        public static ExpenseResponse NoComment => new ExpenseResponse("There is no comment");

        public static ExpenseResponse NoMatchingCurrency =>
            new ExpenseResponse("The currency of the expense is different from the user one");

        public static ExpenseResponse ExpenseAlreadyExist =>
            new ExpenseResponse("An expense with the same amount and the same date already exist");
    }
}