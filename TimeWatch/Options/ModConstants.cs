namespace TimeWatch.Options;

internal static class ModConstants
{
    public const int TimeUnit = 10;
    public const int MinSeekTime = 1; // 10 min
    public const int MaxSeekTime = 36; // 360 min = 6h
    
    public const int MinStorableTime = 0; // 0 = Unlimited
    public const int MaxStorableTime = 28 * 24 * 100; // 67200 Unit = 40,320,000 min = 672 h = 28 days = 1 season
}