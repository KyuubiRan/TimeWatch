using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using TimeWatch.Utils;

namespace TimeWatch.Widgets;

public class TimeWatchOnInventoryMenu : IDisposable
{
    private bool _rendered = false;

    private static IModHelper Helper => ModHelpers.Helper;

    private PerScreen<ClickableTextureComponent> _timeWatch = new()
    {
        Value = new ClickableTextureComponent(
            new Rectangle(0, 0, 128, 128),
            Helper.ModContent.Load<Texture2D>("assets/image/time_watch.png"),
            Rectangle.Empty, 1f
        )
    };

    private Item? _hoverItem = null;
    private Item? _heldItem = null;

    public TimeWatchOnInventoryMenu()
    {
        Helper.Events.Display.RenderedActiveMenu += OnRenderedActiveMenu;
        Helper.Events.Input.ButtonPressed += OnButtonPressed;
        Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
    }

    private void OnRenderedActiveMenu(object? sender, EventArgs e)
    {
        if (!_rendered)
            return;

        Draw();
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (!_rendered)
            return;
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (Game1.activeClickableMenu is not GameMenu { currentTab: 0 } gameMenu)
        {
            _rendered = false;
            return;
        }

        _rendered = true;

        _hoverItem = UiUtils.GetHoveredItem();
        List<IClickableMenu> menuList = gameMenu.pages;
        if (menuList[0] is InventoryPage)
        {
            _heldItem = Game1.player.CursorSlotItem;
        }
    }

    private void Draw()
    {
        if (!_rendered)
            return;
        // TODO
    }

    public void Dispose()
    {
        _rendered = false;
        Helper.Events.Display.RenderedActiveMenu -= OnRenderedActiveMenu;
        Helper.Events.Input.ButtonPressed -= OnButtonPressed;
        Helper.Events.GameLoop.UpdateTicked -= OnUpdateTicked;

        GC.SuppressFinalize(this);
    }
}