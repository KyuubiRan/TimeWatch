using StardewModdingAPI;
using TimeWatch.Options;

namespace TimeWatch.Utils;

internal static class ModHelpers
{
    internal static IMonitor Monitor { get; set; }
    internal static IModHelper Helper { get; set; }
    internal static ModConfig Config { get; set; }
}