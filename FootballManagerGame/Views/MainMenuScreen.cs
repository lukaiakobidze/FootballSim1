using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;

namespace FootballManagerGame.Views;

public class MainMenuScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private int _selectionIndex = 0;
    private List<string> _strings;
    public MainMenuScreen(SpriteFont font, GraphicsDeviceManager graphics)
    {
        _font = font;
        _graphics = graphics;
        _strings = new List<string>() { "New Game", "Exit" };
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int i = 0; i < 2; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 100 + i * 30), color);
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Up))
        {
            _selectionIndex = Math.Max(0, _selectionIndex - 1);
        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            _selectionIndex = Math.Min(1, _selectionIndex + 1);
        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            if (_selectionIndex == 0)
            {
                GameState gameState = new GameState();
                ScreenManager.Instance.AddScreen("NewGame", new NewGameScreen(gameState, _font, _graphics));
                ScreenManager.Instance.ChangeScreen("NewGame");
            }
            if (_selectionIndex == 1)
            {
                Game1.ExitGame = true;
            }

        }
    }
}