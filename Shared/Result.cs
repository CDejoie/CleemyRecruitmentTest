using System;
using System.Diagnostics.CodeAnalysis;

namespace Shared
{
    // Extract from https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/Result.cs
    // It exist a nugget package create by the same author which include it : CSharpFunctionalExtensions

    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public bool Failure => !Success;

        protected Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok()
        {
            return new Result(true, String.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, String.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.Failure)
                    return result;
            }

            return Ok();
        }
    }


    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get => _value;
            [param: AllowNull] private set => _value = value;
        }

        protected internal Result([AllowNull] T value, bool success, string error)
            : base(success, error)
        {
            Value = value;
        }
    }
}