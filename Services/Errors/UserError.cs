using Shared;

namespace Services.Errors
{
    public class UserError : Error
    {
        private UserError(string message) : base(message)
        {
        }

        public static UserError UserNotFound => new UserError("User not found");
    }
}