using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;
using FootballManagerGame.Helpers;

namespace FootballManagerGame.Views;

public class MainMenuScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private GameDataService _gameDataService;
    private List<Texture2D> _textures;
    private int _selectionIndex = 0;
    private List<string> _strings;
    public MainMenuScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _shapes = shapes;
        _textures = textures;
        _strings = new List<string>() {"New Game", "Load Game", "Exit"};
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int i = 0; i < _strings.Count; i++)
        {
            Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 100 + i * 30), color);
        }
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
                ScreenManager.Instance.AddScreen("NewGameSave", new NewGameSaveScreen(_font, _graphics, _gameDataService, _shapes, _textures));
                ScreenManager.Instance.ChangeScreen("NewGameSave");
            }
            else if (_selectionIndex == 1){
                ScreenManager.Instance.AddScreen("LoadGameSave", new LoadGameSaveScreen(_font, _graphics, _gameDataService, _shapes, _textures));
                ScreenManager.Instance.ChangeScreen("LoadGameSave");
            }
            else if (_selectionIndex == _strings.Count - 1)
            {
                Game1.ExitGame = true;
            }

        }
    }
}