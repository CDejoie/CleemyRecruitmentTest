using Shared.Enums;

namespace Repositories.Dtos
{
    public class DbUser
    {
        public long Id { get; set; }
        
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public Currency Currency { get; set; }
    }
}