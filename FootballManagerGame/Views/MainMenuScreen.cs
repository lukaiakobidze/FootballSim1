using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;

namespace FootballManagerGame.Views;

public class MainMenuScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;

    public MainMenuScreen(GameState gameState, SpriteFont font)
    {
        _gameState = gameState;
        _font = font;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.D1) || inputState.IsKeyPressed(Keys.NumPad1))
        {
            ScreenManager.Instance.ChangeScreen("NewGame");
        }
        if (inputState.IsKeyPressed(Keys.NumPad0))
        {
            Game1.ExitGame = true;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Football Manager Game", new Vector2(100, 100), Color.White);
        spriteBatch.DrawString(_font, "1. New Game", new Vector2(100, 150), Color.White);
        spriteBatch.DrawString(_font, "0. Exit", new Vector2(100, 200), Color.White);
        spriteBatch.End();
    }


}