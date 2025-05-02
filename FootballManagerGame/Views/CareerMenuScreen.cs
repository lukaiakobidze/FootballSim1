using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;
using System.Linq;
using FootballManagerGame.Helpers;
using MonoGame.Extended;

namespace FootballManagerGame.Views;

public class CareerMenuScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private GameDataService _gameDataService;
    private GameState _gameState;
    private List<Texture2D> _textures;
    private int _selectionIndex = 0;
    private Fixture _nextGame;
    private List<string> _strings;
    public CareerMenuScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, GameState gameState, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _gameState = gameState;
        _shapes = shapes;
        _textures = textures;
        _strings = new List<string>() {"Advance", "Team Tactics", "Player Managment", "League Table", "All Fixtures", "Main Menu"};
        SetNextGame();
        
        
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _gameState.PlayerTeam?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);
        for (int i = 0; i < _strings.Count; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 100 + i * 30), color);
        }
        spriteBatch.DrawString(_font, $"{_gameState.CurrentDate.Day}  {_gameState.CurrentDate.ToString("MMMM")}  {_gameState.CurrentDate.Year}", new Vector2(_graphics.GraphicsDevice.Viewport.Width - 250, 50), Color.White);
        spriteBatch.DrawString(_font, $"Next game: {_nextGame.Team1.Name} vs {_nextGame.Team2.Name} | {_nextGame.Date.Day} {_nextGame.Date.ToString("MMMM")} {_nextGame.Date.Year}", new Vector2(100, _graphics.GraphicsDevice.Viewport.Height - 50), Color.White);
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

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            if (_selectionIndex == 0)
            {
                if(_gameState.PlayerLeague.AllFixtures.Any(matchday => matchday.Any(f => f.Date == _gameState.CurrentDate))){
                    var todaysFixtures = _gameState.PlayerLeague.AllFixtures
                        .FirstOrDefault(matchday => matchday.Any(f => f.Date == _gameState.CurrentDate));
                        foreach (var fixture in todaysFixtures)
                        {
                            DataGenerator.SimFixture(fixture);
                        }
                    _gameState.CurrentDate = _gameState.CurrentDate.AddDays(1);
                }
                else{
                    _gameState.CurrentDate = _gameState.CurrentDate.AddDays(1);
                }
                SetNextGame();
            }
            else if (_selectionIndex == 1)
            {
                _gameState.TeamSelected = _gameState.PlayerTeam;
                ScreenManager.Instance.AddScreen("TeamStrategyView", new TeamStrategyScreen(_gameState, _font, _graphics, _shapes, _textures));
                ScreenManager.Instance.ChangeScreen("TeamStrategyView");
            }
            else if (_selectionIndex == 2)
            {
                _gameState.TeamSelected = _gameState.PlayerTeam;
                ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(_gameState, _font, _graphics, "CareerMenu"));
                ScreenManager.Instance.ChangeScreen("TeamView");
            }
            else if (_selectionIndex == 3)
            {
                _gameState.LeagueSelected = _gameState.PlayerLeague;
                ScreenManager.Instance.AddScreen("TableView", new TableViewScreen(_font, _graphics, _gameDataService, _gameState));
                ScreenManager.Instance.ChangeScreen("TableView");
            }
            else if (_selectionIndex == 4)
            {
                _gameState.LeagueSelected = _gameState.PlayerLeague;
                ScreenManager.Instance.AddScreen("FixturesView", new FixturesViewScreen(_font, _graphics, _gameState));
                ScreenManager.Instance.ChangeScreen("FixturesView");
            }
            else if (_selectionIndex == _strings.Count - 1)
            {
                _gameDataService.SaveGame(_gameState, $"Save{_gameState.SaveSlot}");
                ScreenManager.Instance.ChangeScreen("MainMenu");
            }
        }

        if (inputState.IsKeyPressed(Keys.Escape)){
            _gameDataService.SaveGame(_gameState, $"Save{_gameState.SaveSlot}");
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }
    }


    public void SetNextGame()
    {
        _nextGame = _gameState.PlayerLeague.AllFixtures
            .SelectMany(day => day)
            .Where(f =>
                f.Date >= _gameState.CurrentDate &&
                (f.Team1.Name == _gameState.PlayerTeam.Name || f.Team2.Name == _gameState.PlayerTeam.Name))
            .OrderBy(f => f.Date)
            .FirstOrDefault();

        // Console.WriteLine(_nextGame != null
        //     ? $"Next game found: {_nextGame.Team1.Name} vs {_nextGame.Team2.Name} on {_nextGame.Date.ToShortDateString()}"
        //     : "No upcoming match found.");
    }
}