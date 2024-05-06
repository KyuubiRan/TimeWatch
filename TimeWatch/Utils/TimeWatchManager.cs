using Newtonsoft.Json;
using StardewValley;

namespace TimeWatch.Utils;

internal static class TimeWatchManager
{
    public static readonly Dictionary<long, Data.MagicTimeWatch> TimeWatches = new();
    private const string Key = "kyuubiran.TimeWatch/TimeWatchData";

    public static Data.MagicTimeWatch GetTimeWatch(long id)
    {
        return TimeWatches.TryGetValue(id, out var watch)
            ? watch
            : TimeWatches[id] = new Data.MagicTimeWatch(Game1.getFarmer(id));
    }

    public static Data.MagicTimeWatch CurrentPlayerTimeWatch => GetTimeWatch(Game1.player);

    public static Data.MagicTimeWatch GetTimeWatch(Farmer player)
    {
        return GetTimeWatch(player.UniqueMultiplayerID);
    }

    public static int AddTime(Farmer player, int cnt)
    {
        return GetTimeWatch(player).Add(cnt);
    }

    public static void OnSave()
    {
        foreach (var farmer in Game1.getAllFarmers())
        {
            if (TimeWatches.TryGetValue(farmer.UniqueMultiplayerID, out var watch))
            {
                farmer.modData[Key] = JsonConvert.SerializeObject(watch);
            }
        }
    }

    public static void OnLoad()
    {
        foreach (var farmer in Game1.getAllFarmers())
        {
            if (!farmer.modData.TryGetValue(Key, out var data)) continue;

            try
            {
                TimeWatches[farmer.UniqueMultiplayerID] = JsonConvert.DeserializeObject<Data.MagicTimeWatch>(data)!;
            }
            catch
            {
                TimeWatches[farmer.UniqueMultiplayerID] = new Data.MagicTimeWatch(farmer);
            }
        }
    }
}