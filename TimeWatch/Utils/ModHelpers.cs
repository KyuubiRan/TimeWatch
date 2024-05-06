using StardewModdingAPI;
using TimeWatch.Options;

namespace TimeWatch.Utils;

internal static class ModHelpers
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal static IMonitor Monitor { get; set; }
    internal static IModHelper Helper { get; set; }
    internal static ModConfig Config { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}