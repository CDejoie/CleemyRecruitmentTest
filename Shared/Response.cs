namespace Shared
{
    public abstract class Response
    {
        protected Response(string value)
        {
            Value = value;
        }

        private static string Value { get; set; }

        public static implicit operator string(Response category)
        {
            return Value;
        }
    }
}