using Shared.Enums;

namespace Repositories.Dtos
{
    public class DbUser
    {
        public long Id { get; set; }
        
        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public Currency Currency { get; set; }
    }
}