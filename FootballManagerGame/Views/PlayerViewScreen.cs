using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
namespace FootballManagerGame.Views;

public class PlayerViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private Player _player;
    public PlayerViewScreen(GameState gameState, SpriteFont font, Player player)
    {
        _gameState = gameState;
        _font = font;
        _player = player;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Player: " + _player.Name ?? "No Player Selected", new Vector2(100, 50), Color.White);

        if (_player != null)
        {
            int y = 100;
            foreach (var att in _player.Attributes)
            {
                spriteBatch.DrawString(_font, $"{att.Key}: {att.Value}", new Vector2(100, y), Color.White);
                y += 30;
            }
        }

        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Escape))
        {
            ScreenManager.Instance.ChangeScreen("TeamView");
            _player = null;
        }
    }

    
}