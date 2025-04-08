using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
namespace FootballManagerGame.Views;


public class TeamViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;

    public TeamViewScreen(GameState gameState, SpriteFont font)
    {
        _gameState = gameState;
        _font = font;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Team: " + _gameState.PlayerTeam?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);

        if (_gameState.PlayerTeam != null)
        {
            int y = 100;
            foreach (var player in _gameState.PlayerTeam.Players)
            {
                string positions = string.Join("/", player.Positions);
                spriteBatch.DrawString(_font, $"{player.Name} - {positions} - Age: {player.Age}", new Vector2(100, y), Color.White);
                y += 30;
            }
        }

        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.NumPad0))
        {
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }
    }
}