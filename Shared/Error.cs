namespace Shared
{
    public abstract class Error
    {
        protected Error(string message)
        {
            Message = message;
        }

        private static string Message { get; set; }

        public static implicit operator string(Error category)
        {
            return Message;
        }
    }
}