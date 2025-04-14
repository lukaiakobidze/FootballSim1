using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;

namespace FootballManagerGame.Views;

public class CareerMenuScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private GameDataService _gameDataService;
    private GameState _gameState;
    private int _selectionIndex = 0;
    private List<string> _strings;
    public CareerMenuScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, GameState gameState)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _gameState = gameState;
        _strings = new List<string>() {"My Team", "League Table", "Main Menu"};
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Team: " + _gameState.PlayerTeam?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);
        for (int i = 0; i < _strings.Count; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 100 + i * 30), color);
        }
        spriteBatch.DrawString(_font, $"{_gameState.CurrentDate.Day}/{_gameState.CurrentDate.Month}/{_gameState.CurrentDate.Year}", new Vector2(_graphics.GraphicsDevice.Viewport.Width - 200, 100), Color.White);
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Up))
        {
            if (_selectionIndex == 0)
            {
                _selectionIndex = _strings.Count - 1;
            }
            else
            {
                _selectionIndex = Math.Max(0, _selectionIndex - 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            if (_selectionIndex == _strings.Count - 1)
            {
                _selectionIndex = 0;
            }
            else
            {
                _selectionIndex = Math.Min(_strings.Count - 1, _selectionIndex + 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Escape)){
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            if (_selectionIndex == 0)
            {
                _gameState.TeamSelected = _gameState.PlayerTeam;
                ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(_gameState, _font, _graphics, "CareerMenu"));
                ScreenManager.Instance.ChangeScreen("TeamView");
            }
            else if (_selectionIndex == 1)
            {
                _gameState.LeagueSelected = _gameState.PlayerLeague;
                ScreenManager.Instance.AddScreen("TableView", new TableViewScreen(_font, _graphics, _gameDataService, _gameState));
                ScreenManager.Instance.ChangeScreen("TableView");
            }
            else if (_selectionIndex == _strings.Count - 1)
            {
                ScreenManager.Instance.ChangeScreen("MainMenu");
            }

        }
    }
}