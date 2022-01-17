using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Dtos;
using Services.Abstractions.Entities;
using Services.Abstractions.Repositories.Contracts;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpensesContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ExpensesContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        private User ConvertFromDb(DbUser dbUser)
        {
            return this._mapper.Map<User>(dbUser);
        }

        public async Task<User?> GetUser(long id)
        {
            DbUser dbResult = await this._context.Users.FirstOrDefaultAsync(expense => expense.Id == id);
            
            return dbResult == null ? default : this.ConvertFromDb(dbResult);
        }
    }
}