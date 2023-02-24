namespace DistanceCalculator.Core.Common.Extensions;

public static class EqualityExtensions
{
    public static bool NotEqual<T>(this T left, T right)
        where T : struct
    {
        return !left.Equals(right);
    }

    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }

    public static bool IsNull<T>(this T? source)
        where T : class
    {
        return source is null;
    }

    public static bool IsNotNull<T>(this T? source)
        where T : class
    {
        return source is not null;
    }
}