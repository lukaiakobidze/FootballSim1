using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FootballManagerGame.Views;
public class ScreenManager
{
    private static ScreenManager _instance;
    public static ScreenManager Instance => _instance ??= new ScreenManager();

    private Dictionary<string, Screen> _screens = new Dictionary<string, Screen>();
    public Screen CurrentScreen { get; private set; }

    public void AddScreen(string name, Screen screen)
    {
        _screens[name] = screen;
    }

    public void ChangeScreen(string screenName)
    {
        if (_screens.ContainsKey(screenName))
            CurrentScreen = _screens[screenName];
    }

    public void Update(GameTime gameTime)
    {
        CurrentScreen?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentScreen?.Draw(spriteBatch);
    }
}