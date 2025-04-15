using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;
using System.Linq;
using Microsoft.VisualBasic;

namespace FootballManagerGame.Views;

public class TableViewScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private GameDataService _gameDataService;
    private GameState _gameState;
    private int _selectionIndex = 0;
    private List<List<string>> TableList;
    private int x, y;
    public TableViewScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, GameState gameState)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _gameState = gameState;
        x = 100;
        y = 100; 

        TableList = _gameState.LeagueSelected.Table
        .Select(Table => new List<string> { Table.Key }.Concat(Table.Value.Select(i => i.ToString())).ToList())
        .OrderByDescending(entry => int.Parse(entry[5]))
        .ThenByDescending(entry => int.Parse(entry[8]))
        .ToList();
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        y = 70;
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, _gameState.LeagueSelected.Name, new Vector2(x, y - 20), Color.White);
        spriteBatch.DrawString(_font, "Team", new Vector2(x, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"P", new Vector2(x + 300, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"W", new Vector2(x + 350, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"D", new Vector2(x + 400, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"L", new Vector2(x + 450, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"Pts", new Vector2(x + 500, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"GF", new Vector2(x + 550, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"GA", new Vector2(x + 600, y + 30), Color.White);
        spriteBatch.DrawString(_font, $"+/-", new Vector2(x + 650, y + 30), Color.White);
        
        int i = 0;
        foreach (var team in TableList)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            if (team[0] == _gameState.PlayerTeam.Name){ color = Color.Cyan; }

            spriteBatch.DrawString(_font, $"{i + 1}.", new Vector2(x - 60, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[0]}", new Vector2(x, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[1]}", new Vector2(x + 300, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[2]}", new Vector2(x + 350, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[3]}", new Vector2(x + 400, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[4]}", new Vector2(x + 450, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[5]}", new Vector2(x + 500, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[6]}", new Vector2(x + 550, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[7]}", new Vector2(x + 600, y + 60), color);
            spriteBatch.DrawString(_font, $"{team[8]}", new Vector2(x + 650, y + 60), color);
    
            y += 30;
            i++;
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Up))
        {
            if (_selectionIndex == 0)
            {
                _selectionIndex = _gameState.LeagueSelected.teams.Count - 1;
            }
            else
            {
                _selectionIndex = Math.Max(0, _selectionIndex - 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            if (_selectionIndex == _gameState.LeagueSelected.teams.Count - 1)
            {
                _selectionIndex = 0;
            }
            else
            {
                _selectionIndex = Math.Min(_gameState.LeagueSelected.teams.Count - 1, _selectionIndex + 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            string targetName = TableList[_selectionIndex][0];
            _gameState.TeamSelected = _gameState.LeagueSelected.teams.FirstOrDefault(t => t.Name == targetName);
            ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(_gameState, _font, _graphics, "TableView")); 
            ScreenManager.Instance.ChangeScreen("TeamView");

        }

        if (inputState.IsKeyPressed(Keys.Escape)){
            _gameState.LeagueSelected = null;
            ScreenManager.Instance.ChangeScreen("CareerMenu");
        }
    }
}