using CleemyRecruitmentTest.ViewModels;
using Services.Entities;
using Shared;

namespace CleemyRecruitmentTest.Mappers
{
    public class GetExpenseViewModelFromGetExpenseMapper : IMapper<GetExpense, GetExpenseViewModel>
    {
        public GetExpenseViewModel Map(GetExpense getEntity)
        {
            return new GetExpenseViewModel()
            {
                User = getEntity.User,
                Date = getEntity.Date,
                Type = getEntity.Type.ToString(),
                Amount = getEntity.Amount,
                Currency = getEntity.Currency.ToString(),
                Comment = getEntity.Comment,
            };
        }
    }
}