using System.Linq;
using Repositories.Contracts;
using Repositories.Dtos;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpensesContext _context;

        public UserRepository(ExpensesContext context)
        {
            this._context = context;
        }

        public DbUser GetUser(long id)
        {
            return this._context.Users.FirstOrDefault(expense => expense.Id == id);
        }
    }
}