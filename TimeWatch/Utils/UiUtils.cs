using StardewValley;
using StardewValley.Menus;

namespace TimeWatch.Utils;

internal static class UiUtils
{
    /**
     * Taken from https://github.com/Annosz/UIInfoSuite2/blob/master/UIInfoSuite2/Infrastructure/Tools.cs#L114
     */
    public static Item? GetHoveredItem()
    {
        Item? hoverItem = null;

        if (Game1.activeClickableMenu == null && Game1.onScreenMenus != null)
        {
            hoverItem = Game1.onScreenMenus.OfType<Toolbar>().Select(tb => tb.hoverItem).FirstOrDefault(hi => hi is not null);
        }

        hoverItem = Game1.activeClickableMenu switch
        {
            GameMenu gameMenu when gameMenu.GetCurrentPage() is InventoryPage inventory => inventory.hoveredItem,
            ItemGrabMenu itemMenu => itemMenu.hoveredItem,
            _ => hoverItem
        };

        return hoverItem;
    }
}