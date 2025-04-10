using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Data;
using System.Collections.Generic;
using FootballManagerGame.Models;

namespace FootballManagerGame.Views;

public class LoadGameSaveScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private GameDataService _gameDataService;
    private List<GameState> _gameStates = new List<GameState>();
    private bool _error = false;
    private int _saveAmount = 5;
    private int _selectionIndex = 0;

    public LoadGameSaveScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        if(_gameDataService.GetSaveGames() != null){
            foreach (string save in _gameDataService.GetSaveGames()){
                _gameStates.Add(_gameDataService.LoadGame(save));
            }
        }
    }
    

    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Select Save Slot To Load:", new Vector2(100, 50), Color.White);
        for (int i = 0; i < _saveAmount; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, $"Save Slot {i+1}", new Vector2(100, 110 + i * 30), color);
            foreach (var state in _gameStates){
                if(i+1 == state.SaveSlot){
                    spriteBatch.DrawString(_font, $"Team: {state.PlayerTeam.Name}", new Vector2(300, 110 + i * 30), color);
                }
            }
        }
        if(_error){
            spriteBatch.DrawString(_font, $"Save is empty!", new Vector2(100, _graphics.GraphicsDevice.Viewport.Height - 60), Color.White);
        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Up))
        {   
            if (_selectionIndex == 0)
            {
                _selectionIndex = _saveAmount - 1;
            }
            else{
                _selectionIndex = Math.Max(0, _selectionIndex - 1);
            }
            
        }

        if (inputState.IsKeyPressed(Keys.Down))
        {
            if (_selectionIndex == _saveAmount - 1)
            {
                _selectionIndex = 0;
            }
            else
            {
                _selectionIndex = Math.Min(_saveAmount - 1, _selectionIndex + 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
            if (_gameDataService.GetSaveGames().Contains($"Save{_selectionIndex + 1}")){
                GameState gameState = _gameDataService.LoadGame($"Save{_selectionIndex + 1}");
                ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(gameState, _font, _graphics));
                ScreenManager.Instance.ChangeScreen("TeamView");
            }
            else{
                _error = true;
            }
            
        }

        if (inputState.IsKeyPressed(Keys.Delete))
        {
            if (_gameDataService.GetSaveGames().Contains($"Save{_selectionIndex + 1}")){
                _gameDataService.DeleteSaveGame($"Save{_selectionIndex + 1}");
                _gameStates = new List<GameState>();
                foreach (string save in _gameDataService.GetSaveGames()){
                    _gameStates.Add(_gameDataService.LoadGame(save));
                }
                
            }
        }

        if (inputState.IsKeyPressed(Keys.Escape))
        {
            ScreenManager.Instance.ChangeScreen("MainMenu");
        }
    }
}