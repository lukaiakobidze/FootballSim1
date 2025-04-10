using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
using System.Collections.Generic;
namespace FootballManagerGame.Views;


public class TeamViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private int _selectedPlayerIndex = 0;
    public TeamViewScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics)
    {
        _graphics = graphics;
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
        var orderedList = _gameState.PlayerTeam.Players.OrderBy(p => p.Positions.First()).ToList();
        if (_gameState.PlayerTeam != null)
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
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Escape))
        {
            
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }
        if (inputState.IsKeyPressed(Keys.Up))
        {
            if (_selectedPlayerIndex == 0)
            {
                _selectedPlayerIndex = _gameState.TeamSelected.Players.Count - 1;
            }
            else{
                _selectedPlayerIndex = Math.Max(0, _selectedPlayerIndex - 1);
            }
        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            if (_selectedPlayerIndex == _gameState.TeamSelected.Players.Count - 1)
            {
                _selectedPlayerIndex = 0;
            }
            else
            {
                _selectedPlayerIndex = Math.Min(_gameState.TeamSelected.Players.Count - 1, _selectedPlayerIndex + 1);
            }
        }
        if (inputState.IsKeyPressed(Keys.Enter))
        {
            var orderedList = _gameState.PlayerTeam.Players.OrderBy(p => p.Positions.First()).ToList();
            _gameState.PlayerSelected = orderedList[_selectedPlayerIndex];
            ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, orderedList[_selectedPlayerIndex]));
            ScreenManager.Instance.ChangeScreen("PlayerView");

        }
    }
}