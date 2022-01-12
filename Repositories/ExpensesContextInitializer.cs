using System.Linq;
using Repositories.Dtos;
using Shared.Enums;

namespace Repositories
{
    public static class ExpensesContextInitializer
    {
        public static void Initialize(ExpensesContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Users.Any())
            {
                return;
            }

            var initialUsers = new DbUser[]
            {
                new DbUser() { LastName = "Stark", FirstName = "Anthony", Currency = Currency.USD },
                new DbUser() { LastName = "Romanova", FirstName = "Natasha", Currency = Currency.RUB },
            };

            context.Users.AddRange(initialUsers);
            context.SaveChanges();
        }
    }
}