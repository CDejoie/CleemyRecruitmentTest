using Shared.Enums;

namespace Services.Abstractions.Entities
{
    public class User
    {
        public long Id { get; set; }
        
        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public Currency Currency { get; set; }
    }
}