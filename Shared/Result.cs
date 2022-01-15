using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Shared
{
    // Inspired from https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/Result.cs
    // It exist a nugget package create by the same author which include it : CSharpFunctionalExtensions

    public class Result
    {
        public bool Success { get; private set; }
        public IList<string> Errors { get; private set; }

        public bool Failure => !Success;

        public Result()
        {
            Success = true;
            Errors = new List<string>();
        }

        protected Result(bool success, string error)
        {
            Success = success;
            Errors = new [] {error};
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

        public static void AddError(Result result, string message)
        {
            if (result.Success) result.Success = false; 
            result.Errors.Add(message);
        }
        
        public static void AddErrors(Result result, IList<string> messages)
        {
            if (result.Success) result.Success = false;
            foreach (string message in messages)
            {
                result.Errors.Add(message);
            }
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