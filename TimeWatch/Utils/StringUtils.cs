namespace TimeWatch.Utils;

public static class StringUtils
{
    public static string Format(this string str, params object[] args) => string.Format(str, args);
}