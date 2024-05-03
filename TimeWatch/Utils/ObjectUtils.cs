// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;

namespace TimeWatch.Utils;

internal static class ObjectUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Let<T, R>(this T thiz, Func<T, R> block) => block(thiz);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Apply<T>(this T thiz, Action<T> block)
    {
        block(thiz);
        return thiz;
    }
}