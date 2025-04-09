using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
using System.Collections.Generic;
namespace FootballManagerGame.Views;


public class NewGameTeamViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private int _selectedPlayerIndex = 0;


    public NewGameTeamViewScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics)
    {
        _gameState = gameState;
        _font = font;
        _graphics = graphics;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Team: " + _gameState.TeamSelected?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);
        var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ToList();
        if (_gameState.TeamSelected != null)
        {
            int y = 100;
            for (int i = 0; i < orderedList.Count; i++)
            {
                Color color = (i == _selectedPlayerIndex) ? Color.Yellow : Color.White;
                Color colorOVR = Color.White;
                string positions = string.Join("/", orderedList[i].Positions);
                spriteBatch.DrawString(_font, $"{orderedList[i].Name} - {positions} - Age: {orderedList[i].Age}", new Vector2(100, y), color);

                if (orderedList[i].Overall < 40) { colorOVR = Color.IndianRed; }
                else if (orderedList[i].Overall < 60) { colorOVR = Color.Orange; }
                else if (orderedList[i].Overall < 70) { colorOVR = Color.Yellow; }
                else if (orderedList[i].Overall < 80) { colorOVR = Color.LightGreen; }
                else if (orderedList[i].Overall < 90) { colorOVR = Color.SpringGreen; }
                else { colorOVR = Color.Cyan; }
                spriteBatch.DrawString(_font, $"{orderedList[i].Overall}", new Vector2(500, y), colorOVR);
                y += 30;
            }
            spriteBatch.DrawString(_font, $"Press SPACE to Pick this team", new Vector2(100, _graphics.GraphicsDevice.Viewport.Height - 90), Color.White);
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Escape))
        {
            _gameState.TeamSelected = null;
            ScreenManager.Instance.ChangeScreen("NewGame");
        }
        if (inputState.IsKeyPressed(Keys.Up))
        {
            _selectedPlayerIndex = Math.Max(0, _selectedPlayerIndex - 1);
        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            _selectedPlayerIndex = Math.Min(_gameState.TeamSelected.Players.Count - 1, _selectedPlayerIndex + 1);
        }
        if (inputState.IsKeyPressed(Keys.Enter))
        {
            var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ToList();
            _gameState.PlayerSelected = orderedList[_selectedPlayerIndex];
            ScreenManager.Instance.AddScreen("NewGamePlayerView", new NewGamePlayerViewScreen(_gameState, _font, orderedList[_selectedPlayerIndex]));
            ScreenManager.Instance.ChangeScreen("NewGamePlayerView");

        }
        if (inputState.IsKeyPressed(Keys.Space))
        {
            _gameState.PlayerTeam = _gameState.TeamSelected;
            ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(_gameState, _font));
            ScreenManager.Instance.ChangeScreen("TeamView");
        }
    }
}