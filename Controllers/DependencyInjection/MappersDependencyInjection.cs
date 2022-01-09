using CleemyRecruitmentTest.Mappers;
using CleemyRecruitmentTest.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Dtos;
using Services.Entities;
using Services.Mappers;
using Shared;

namespace CleemyRecruitmentTest.DependencyInjection
{
    internal static class MappersDependencyInjection
    {
        internal static void Register(IServiceCollection services)
        {
            services.AddScoped<IMapper<GetExpense, GetExpenseViewModel>, GetExpenseViewModelFromGetExpenseMapper>();
            services.AddScoped<IMapper<Expense, DbExpense>, DbExpenseFromExpenseMapper>();
            services.AddScoped<IMapper<DbExpense, Expense>, ExpenseFromDbExpenseMapper>();
            services.AddScoped<IMapper<DbUser, User>, UserFromDbUserMapper>();
        }
    }
}