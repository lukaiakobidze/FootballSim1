using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Data;
using System.Collections.Generic;
using FootballManagerGame.Models;

namespace FootballManagerGame.Views;

public class NewGameScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private GameDataService _gameDataService;
    private int _saveSlot;
    private List<Team> _availableTeams;
    private int _selectedTeamIndex = 0;

    public NewGameScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, int saveSlot)
    {
        _gameState = gameState;
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _saveSlot = saveSlot;
        
        _availableTeams = _gameState.Leagues[0].teams;
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Select Your Team", new Vector2(100, 50), Color.White);


        for (int i = 0; i < _availableTeams.Count; i++)
        {
            Color color = (i == _selectedTeamIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _availableTeams[i].Name, new Vector2(100, 100 + i * 30), color);
        }

        spriteBatch.DrawString(_font, "Press Enter to select team", new Vector2(100, 130 + (_availableTeams.Count * 30)), Color.White);
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Up))
        {   
            if (_selectedTeamIndex == 0)
            {
                _selectedTeamIndex = _availableTeams.Count - 1;
            }
            else{
                _selectedTeamIndex = Math.Max(0, _selectedTeamIndex - 1);
            }
            
        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            if (_selectedTeamIndex == _availableTeams.Count - 1)
            {
                _selectedTeamIndex = 0;
            }
            else
            {
                _selectedTeamIndex = Math.Min(_availableTeams.Count - 1, _selectedTeamIndex + 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            _gameState.TeamSelected = _availableTeams[_selectedTeamIndex];
            ScreenManager.Instance.AddScreen("NewGameTeamView", new NewGameTeamViewScreen(_gameState, _font, _graphics, _gameDataService, _saveSlot));
            ScreenManager.Instance.ChangeScreen("NewGameTeamView");
        }

        if (inputState.IsKeyPressed(Keys.Escape))
        {
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }
    }
}