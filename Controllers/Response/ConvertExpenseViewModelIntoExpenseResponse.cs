namespace CleemyRecruitmentTest.Response
{
    using Shared;

    public class ConvertExpenseViewModelIntoExpenseResponse : Response
    {
        private ConvertExpenseViewModelIntoExpenseResponse(string value) : base(value)
        {
        }

        public static ConvertExpenseViewModelIntoExpenseResponse WrongType =>
            new ConvertExpenseViewModelIntoExpenseResponse("The expense type doesnt match any available type");
        
        public static ConvertExpenseViewModelIntoExpenseResponse WrongCurrency =>
            new ConvertExpenseViewModelIntoExpenseResponse("The currency doesnt match any available currency");
    }
}