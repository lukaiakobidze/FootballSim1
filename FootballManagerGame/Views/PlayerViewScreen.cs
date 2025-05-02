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
    private string OriginScreen;
    public PlayerViewScreen(GameState gameState, SpriteFont font, Player player, string originScreen)
    {
        _gameState = gameState;
        _font = font;
        _player = player;
        OriginScreen = originScreen;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Player:  " + _player.Name ?? "No Player Selected", new Vector2(100, 50), Color.White);
        if (_player != null)
        {
            int i = 0;
            int y = 160;
            int x = 100;
            Color color = Color.White;
            Color colorStats = Color.White;

            string positions = string.Join("/", _player.Positions);
            spriteBatch.DrawString(_font, $"Positions: {positions}", new Vector2(100, y - 60), color);
            spriteBatch.DrawString(_font, $"Overall:", new Vector2(100, y - 30), color);
            spriteBatch.DrawString(_font, $"Phyisical", new Vector2(100, y), color);
            spriteBatch.DrawString(_font, $"Technical", new Vector2(100, y + 150), color);
            spriteBatch.DrawString(_font, $"Mental", new Vector2(400, y), color);
            spriteBatch.DrawString(_font, $"Goalkeeping", new Vector2(400, y + 180), color);

            if (_player.Overall < 40) { color = Color.IndianRed; }
            else if (_player.Overall < 60) { color = Color.Orange; }
            else if (_player.Overall < 70) { color = Color.Yellow; }
            else if (_player.Overall < 80) { color = Color.LightGreen; }
            else if (_player.Overall < 90) { color = Color.SpringGreen; }
            else { color = Color.Cyan; }
            spriteBatch.DrawString(_font, $"{_player.Overall}", new Vector2(x + 180, y - 30), color);

            foreach (var att in _player.Attributes)
            {
                y += 30;
                if (i == 3) { y += 60; }
                else if (i == 13) { y = 190; x = 400;}
                else if (i == 17) { y += 60; }

                if (att.Value < 40) { colorStats = Color.IndianRed; }
                else if (att.Value < 60) { colorStats = Color.Orange; }
                else if (att.Value < 70) { colorStats = Color.Yellow; }
                else if (att.Value < 80) { colorStats = Color.LightGreen; }
                else if (att.Value < 90) { colorStats = Color.SpringGreen; }
                else { colorStats = Color.Cyan; }

                spriteBatch.DrawString(_font, $"{att.Key}", new Vector2(x, y), Color.Silver);
                spriteBatch.DrawString(_font, $"{att.Value}", new Vector2(x + 180, y), colorStats);

                i++;

            }
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Escape))
        {
            _player = null;
            ScreenManager.Instance.ChangeScreen(OriginScreen);
        }
    }


}