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
    private Fixture _fixture;
    private bool _startSim = false;
    private double _time = 0;
    private int _timePrev = 0;
    private int _half = 1;
    private bool _fixtureOver = false;
    private List<int> _fullTime;
    
    public LiveSimScreen(SpriteFont font, GraphicsDeviceManager graphics, GameDataService gameDataService, GameState gameState, ShapeDrawer shapes, List<Texture2D> textures, Fixture fixture)
    {
        _font = font;
        _graphics = graphics;
        _gameDataService = gameDataService;
        _gameState = gameState;
        _shapes = shapes;
        _textures = textures;
        
        _fixture = fixture;
        _fullTime = new List<int>();
        _fullTime.Add(DataGenerator.Random.Next(45, 52));
        _fullTime.Add(DataGenerator.Random.Next(45, 52));
        
    }

    public override void Update(GameTime gameTime)
    {
        if (!_fixtureOver)
        {
            if (_startSim)
            {
                _time += gameTime.ElapsedGameTime.TotalSeconds * 2;

                if ((int)_time != _timePrev)
                {
                    if (_half == 1 && (int)_time == _fullTime[0])
                    {
                        DataGenerator.SimMinute(_fixture, (int)_time, false);
                        _startSim = false;
                        _half = 2;
                        _time = 45;
                    }
                    else if (_half == 2 && (int)_time == _fullTime[1] + 45)
                    {
                        DataGenerator.SimMinute(_fixture, (int)_time, true);
                        _startSim = false;
                        _fixtureOver = true;
                    }
                    else
                    {
                        DataGenerator.SimMinute(_fixture, (int)_time, false);
                    }
                    _timePrev = (int)_time;
                }
    
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.DrawString(_font, $"{(int)_time}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, 100), Color.White);


        spriteBatch.DrawString(_font, $"{_fixture.Team1.Name}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - 400, 150), Color.White, 0f,
            _font.MeasureString($"{_fixture.Team1.Name}") / 2, 1, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, $"{_fixture.Result[0]}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 - 200, 150), Color.White, 0f,
            _font.MeasureString($"{_fixture.Result[0]}") / 2, 1, SpriteEffects.None, 0f);



        spriteBatch.DrawString(_font, $"{_fixture.Team2.Name}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 + 400, 150), Color.White, 0f,
            _font.MeasureString($"{_fixture.Team2.Name}") / 2, 1, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, $"{_fixture.Result[1]}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 + 200, 150), Color.White, 0f,
            _font.MeasureString($"{_fixture.Result[1]}") / 2, 1, SpriteEffects.None, 0f);





        spriteBatch.DrawString(_font, $"{_fixture.team1ChancesCreated} - Chances Created - {_fixture.team2ChancesCreated}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 , 800), Color.White, 0f,
            _font.MeasureString($"{_fixture.team1ChancesCreated} - Chances Created - {_fixture.team2ChancesCreated}") / 2, 1, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, $"{_fixture.team1ShotAttempts} - Shots Attempted - {_fixture.team2ShotAttempts}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 , 830), Color.White, 0f,
            _font.MeasureString($"{_fixture.team1ShotAttempts} - Shots Attempted - {_fixture.team2ShotAttempts}") / 2, 1, SpriteEffects.None, 0f);
        spriteBatch.DrawString(_font, $"{_fixture.team1ShotOnTarget} - Shots On Target - {_fixture.team2ShotOnTarget}", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 , 860), Color.White, 0f,
            _font.MeasureString($"{_fixture.team1ShotOnTarget} - Shots On Target - {_fixture.team2ShotOnTarget}") / 2, 1, SpriteEffects.None, 0f);


        if (_fixture.Goals.Count != 0)
        {
            for (int i = 0; i < _fixture.Goals.Count; i++)
            {
                spriteBatch.DrawString(_font, $"{_fixture.Goals[i].TimeScored} min - {_fixture.Goals[i].PlayerScored.Name} scores for {_fixture.Goals[i].PlayerScored.Team.NameShort}!", new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2 , 200 + i * 30), Color.White, 0f,
                    _font.MeasureString($"{_fixture.Goals[i].TimeScored} min - {_fixture.Goals[i].PlayerScored.Name} scored!") / 2, 1, SpriteEffects.None, 0f);
            }
        }

        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Enter)){
            if (_startSim)
            {
                _startSim = false;
            }
            else
            {
                _startSim = true;
            }
            
        }
        if (inputState.IsKeyPressed(Keys.Escape) && _fixtureOver){
            ScreenManager.Instance.ChangeScreen("CareerMenu");
        }
    }


    
}