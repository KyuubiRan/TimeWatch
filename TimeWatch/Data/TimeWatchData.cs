using Newtonsoft.Json;
using StardewValley;
using TimeWatch.Utils;

namespace TimeWatch.Data;

internal class TimeWatchData
{
    public static int MaxStorableTime => ModHelpers.Config.MaximumStorableTime * 10;
    public static GameTimeSpan MaxStorableTimeSpan => GameTimeSpan.FromMinutes(MaxStorableTime);

    [JsonIgnore] public readonly Farmer Owner;
    [JsonIgnore] public long OwnerId => Owner.UniqueMultiplayerID;
    [JsonIgnore] public GameTimeSpan TimeSpan => GameTimeSpan.FromMinutes(StoredTime);

    public int StoredTime { get; private set; }

    public TimeWatchData(Farmer owner)
    {
        Owner = owner;
        if (MaxStorableTime > 0)
            StoredTime = StoredTime.CoerceIn(0, MaxStorableTime);
    }

    public int CalcCost(int cnt)
    {
        cnt *= 10;

        if (MaxStorableTime == 0)
            return cnt;

        switch (cnt)
        {
            case 0:
                return 0;

            case > 0:
            {
                var maximumAddable = MaxStorableTime - StoredTime;
                return Math.Min(cnt, maximumAddable);
            }
            default:
                return StoredTime > Math.Abs(cnt) ? cnt : -StoredTime;
        }
    }

    public void Clear()
    {
        StoredTime = 0;
    }

    public int CanAdded(int minutes)
    {
        if (minutes == 0)
            return 0;

        var isPlus = minutes > 0;
        int added;
        if (isPlus)
        {
            var maximumAddable = MaxStorableTime - StoredTime;
            added = Math.Min(minutes, maximumAddable);
        }
        else
        {
            added = StoredTime > Math.Abs(minutes) ? minutes : -StoredTime;
        }

        return added;
    }

    public int Add(int minutes)
    {
        if (minutes == 0)
            return 0;

        var isPlus = minutes > 0;
        int added;
        if (isPlus)
        {
            var maximumAddable = MaxStorableTime - StoredTime;
            added = Math.Min(minutes, maximumAddable);
        }
        else
        {
            added = StoredTime > Math.Abs(minutes) ? minutes : -StoredTime;
        }

        StoredTime += added;

        // Return retained time
        return minutes - added;
    }
}