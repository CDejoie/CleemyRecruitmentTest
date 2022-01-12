namespace Services.Response
{
    using Shared;

    public class UserResponse : Response
    {
        private UserResponse(string value) : base(value)
        {
        }

        public static UserResponse UserNotFound => new UserResponse("User not found");
    }
}