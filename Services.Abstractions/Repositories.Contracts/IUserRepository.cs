using System.Threading.Tasks;
using Services.Abstractions.Entities;

namespace Services.Abstractions.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User?> GetUser(long id);
    }
}