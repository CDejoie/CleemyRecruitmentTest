using AutoMapper;
using CleemyRecruitmentTest.ViewModels;
using Services.Abstractions.Entities;

namespace CleemyRecruitmentTest.Mappers
{
    public class ControllersProfile : Profile
    {
        public ControllersProfile()
        {
            CreateMap<ExpenseViewModel, Expense>();
            CreateMap<GetExpense, GetExpenseViewModel>();
        }
    }
}