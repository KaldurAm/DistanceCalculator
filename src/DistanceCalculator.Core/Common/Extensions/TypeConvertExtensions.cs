using Newtonsoft.Json;

namespace DistanceCalculator.Core.Common.Extensions;

public static class TypeConvertExtensions
{
    /// <summary>
    /// converts model to json
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static string ToJson<TSource>(this TSource source)
    {
        return JsonConvert.SerializeObject(source);
    }

    /// <summary>
    /// converts json to generic class
    /// allow return null
    /// </summary>
    /// <param name="json"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public static TModel? ConvertTo<TModel>(this string json)
    {
        return JsonConvert.DeserializeObject<TModel>(json);
    }
}