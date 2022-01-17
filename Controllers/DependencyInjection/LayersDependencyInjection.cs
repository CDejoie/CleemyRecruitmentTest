using Microsoft.Extensions.DependencyInjection;
using Repositories.Repositories;
using Services.Abstractions.Repositories.Contracts;
using Services.Contracts;
using Services.Services;

namespace CleemyRecruitmentTest.DependencyInjection
{
    internal static class LayersDependencyInjection
    {
        internal static void RegisterLayers(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}