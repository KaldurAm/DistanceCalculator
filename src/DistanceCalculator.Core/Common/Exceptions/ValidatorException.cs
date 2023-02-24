using FluentValidation;
using FluentValidation.Results;

namespace DistanceCalculator.Core.Common.Exceptions;

public class ValidatorException : ValidationException
{
    /// <inheritdoc />
    public ValidatorException(IEnumerable<ValidationFailure> errors) : base(errors) { }
}