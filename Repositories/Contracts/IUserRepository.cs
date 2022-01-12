using Repositories.Dtos;

namespace Repositories.Contracts
{
    public interface IUserRepository
    {
        DbUser GetUser(long id);
    }
}