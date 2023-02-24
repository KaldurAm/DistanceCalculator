using System;
using DistanceCalculator.Core.ValueObjects;

namespace DistanceCalculator.Core.Common.Primitives
{
    public class Result
    {
        /// <summary>
        /// initialize a new instance of the <see cref="Result"/> class with the specified parameters
        /// </summary>
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// gets a value indicating whether the result is a success result
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// gets a value indicating whether the result is a failure result
        /// </summary>
        public bool IsFailed => !IsSuccess;

        /// <summary>
        /// gets the error
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// returns a success <see cref="Result"/> with the success flag set
        /// </summary>
        /// <returns></returns>
        public static Result Success()
        {
            return new Result(true, Error.None);
        }

        /// <summary>
        /// returns a success <see cref="Result{TValue}"/> with the specified value
        /// </summary>
        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, true, Error.None);
        }

        /// <summary>
        /// creates a new <see cref="Result{TValue}"/> with the specified nullable value and the specified error
        /// </summary>
        public static Result<TValue> Create<TValue>(TValue? value, Error error)
            where TValue : class
        {
            return value is null ? Failure<TValue>(error) : Success(value);
        }

        /// <summary>
        /// returns a failure <see cref="Result"/> with the specified error
        /// </summary>
        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }

        /// <summary>
        /// returns a failure <see cref="Result{TValue}"/> with the specified error
        /// </summary>
        public static Result<TValue> Failure<TValue>(Error error)
        {
            return new Result<TValue>(default!, false, error);
        }

        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailed)
                    return result;
            }

            return Success();
        }
    }
}