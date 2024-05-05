using Newtonsoft.Json;
using StardewValley;
using TimeWatch.Utils;

namespace TimeWatch.Data;

internal class MagicTimeWatch
{
    public static int MaxStorableTime => ModHelpers.Config.MaximumStorableTime * 10;
    public static GameTimeSpan MaxStorableTimeSpan => GameTimeSpan.FromMinutes(MaxStorableTime);

    [JsonIgnore] public readonly Farmer Owner;
    [JsonIgnore] public long OwnerId => Owner.UniqueMultiplayerID;
    [JsonIgnore] public GameTimeSpan TimeSpan => GameTimeSpan.FromMinutes(StoredTime);

    public int StoredTime { get; private set; }

    public MagicTimeWatch(Farmer owner)
    {
        Owner = owner;
        if (MaxStorableTime > 0)
            StoredTime = StoredTime.CoerceIn(0, MaxStorableTime);
    }

    /// <summary>
    /// Calculate the maximum time can be stored or released.
    /// </summary>
    /// <param name="cnt">Time unit</param>
    /// <returns>Maximum time(minutes) consumable</returns>
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

    /// <summary>
    /// Seek time by time unit, can be negative.
    /// </summary>
    /// <param name="cnt">Skeed time unit</param>
    /// <param name="performUpdate">Use Game1.performTenMinuteClockUpdate instead</param>
    /// <param name="showNotify">Show HUD message</param>
    /// <returns>Time minutes seeked, 0 = failed</returns>
    public int Seek(int cnt, bool performUpdate = false, bool showNotify = true)
    {
        // ignored
        if (cnt == 0)
            return 0;

        var cost = CalcCost(cnt);
        // Check cost == cnt
        if (cost != cnt * 10)
        {
            if (showNotify)
            {
                Game1.addHUDMessage(
                    HUDMessage.ForCornerTextbox(cnt > 0
                        ? I18n.Message_StoreFailedMaximum().Format(TimeSpan, MaxStorableTimeSpan)
                        : I18n.Message_ReleaseFailedNotEnough().Format(TimeSpan, GameTimeSpan.FromMinutes(cnt * 10))));
            }

            return 0;
        }

        // check after seek world time is in [06:00, 24:00]
        var t = (GameTimeSpan.WorldNow + GameTimeSpan.FromMinutes(cost)).Count;
        switch (t)
        {
            case > 2400 when cost > 0:
                if (showNotify)
                    Game1.addHUDMessage(HUDMessage.ForCornerTextbox(I18n.Message_StoreFailedExceedZeroOClock()));
                return 0;
            case < 600 when cost < 0:
                if (showNotify)
                    Game1.addHUDMessage(HUDMessage.ForCornerTextbox(I18n.Message_ReleaseFailedEarlierSixOClock()));
                return 0;
        }

        var canSeekMinutes = GameTimeUtils.CanSeek(cost);
        var canAddMinutes = CanAdded(cost);

        // Check same value, if same then seek time
        if (canSeekMinutes == canAddMinutes)
        {
            Add(canSeekMinutes);

            if (performUpdate && cnt > 0)
                GameTimeUtils.PerformUpdateTime(canSeekMinutes / 10);
            else
                GameTimeUtils.SeekTime(canSeekMinutes);

            if (showNotify)
            {
                Game1.addHUDMessage(
                    HUDMessage.ForCornerTextbox(
                        (cnt > 0 ? I18n.Message_TimeStored() : I18n.Message_TimeReleased())
                        .Format(GameTimeSpan.FromMinutes(canSeekMinutes), GameTimeSpan.FromMinutes(StoredTime)))
                );
            }

            return canSeekMinutes;
        }
        else if (canAddMinutes > canSeekMinutes)
        {
            if (showNotify)
            {
                Game1.addHUDMessage(
                    HUDMessage.ForCornerTextbox(cnt > 0
                        ? I18n.Message_StoreFailedExceedZeroOClock()
                        : I18n.Message_ReleaseFailedEarlierSixOClock()));
            }

            return 0;
        }

        return 0;
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