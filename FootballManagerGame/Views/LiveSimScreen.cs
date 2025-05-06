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

public class LiveSimScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private GameDataService _gameDataService;
    private GameState _gameState;
    private List<Texture2D> _textures;
    private int _selectionIndex = 0;
    private bool _startSim = false;
    private double _time = 0;
    
    public LiveSimScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, GameState gameState, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _gameState = gameState;
        _shapes = shapes;
        _textures = textures;
        
        
        
    }

    public override void Update(GameTime gameTime)
    {

        if (_time >= 90.0){
            _startSim = false;
        }

        if (_startSim){
            _time += gameTime.ElapsedGameTime.TotalSeconds * 2;
        }
        

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.DrawString(_font, $"{(int)_time}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, 100), Color.White);

        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Enter)){
            _startSim = true;
        }
        if (inputState.IsKeyPressed(Keys.Escape)){
            ScreenManager.Instance.ChangeScreen("CareerMenu");
        }
    }


    
}