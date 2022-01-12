using Repositories.Dtos;
using Services.Entities;
using Shared;

namespace Services.Mappers
{
    public class UserFromDbUserMapper : IMapper<DbUser, User>
    {
        public User Map(DbUser dbEntity)
        {
            return new User()
            {
                Id = dbEntity.Id,
                LastName = dbEntity.LastName,
                FirstName = dbEntity.FirstName,
                Currency = dbEntity.Currency,
            };
        }
    }
}