using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Data;
using System.Collections.Generic;
using FootballManagerGame.Models;
using FootballManagerGame.Helpers;

namespace FootballManagerGame.Views;

public class NewGameSaveScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private GameDataService _gameDataService;
    private List<Texture2D> _textures;
    private List<GameState> _gameStates = new List<GameState>();
    private int _saveAmount = 5;
    private int _selectionIndex = 0;

    public NewGameSaveScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _shapes = shapes;
        _textures = textures;
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
        spriteBatch.DrawString(_font, "Select Save Slot:", new Vector2(100, 50), Color.White);
        for (int i = 0; i < _saveAmount; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, $"Save Slot {i+1}", new Vector2(100, 110 + i * 30), color);
            foreach (var state in _gameStates)
            {
                if (state != null && i + 1 == state.SaveSlot && state.PlayerTeam != null)
                {
                    spriteBatch.DrawString(_font, $"Team: {state.PlayerTeam.Name}", new Vector2(300, 110 + i * 30), color);
                }
            }
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
            GameState gameState = new GameState();
            gameState.InitializeNewGame(_selectionIndex + 1);
            ScreenManager.Instance.AddScreen("NewGame", new NewGameScreen(gameState, _font, _graphics, _gameDataService, _shapes, _textures, _selectionIndex + 1));
            ScreenManager.Instance.ChangeScreen("NewGame");
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