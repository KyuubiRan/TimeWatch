namespace TimeWatch.Utils;

internal static class NumberUtils
{
    public static T CoerceIn<T>(this T value, T min, T max) where T : struct, IComparable<T> => value switch
    {
        _ when value.CompareTo(min) < 0 => min,
        _ when value.CompareTo(max) > 0 => max,
        _ => value,
    };
}