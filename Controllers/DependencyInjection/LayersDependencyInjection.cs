using Microsoft.Extensions.DependencyInjection;
using Repositories.Contracts;
using Repositories.Repositories;
using Services.Contracts;
using Services.Services;

namespace CleemyRecruitmentTest.DependencyInjection
{
    internal static class LayersDependencyInjection
    {
        internal static void Register(IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}