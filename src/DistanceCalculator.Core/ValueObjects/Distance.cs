using DistanceCalculator.ValueObjects;

namespace DistanceCalculator.Core.ValueObjects;

public sealed class Distance : ValueObject
{
    public readonly double Value;

    public Distance(double value)
    {
        Value = value;
    }

    public static Distance Empty => new(0);

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}