using System.Collections.Generic;
using DistanceCalculator.Results.Enums;
using DistanceCalculator.ValueObjects;

namespace DistanceCalculator.Core.ValueObjects
{
    public class Error : ValueObject
    {
        /// <summary>
        /// initializes a new instance of the <see cref="Error" /> class
        /// </summary>
        public Error(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// gets the error code
        /// </summary>
        public ErrorCode Code { get; }

        /// <summary>
        /// gets the error message
        /// </summary>
        public string Message { get; }

        public static Error None => new Error(ErrorCode.NONE, string.Empty);

        public static implicit operator ErrorCode(Error error)
        {
            return error?.Code ?? ErrorCode.NONE;
        }

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }
    }
}