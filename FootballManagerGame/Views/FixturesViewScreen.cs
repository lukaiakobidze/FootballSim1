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

public class FixturesViewScreen : Screen
{
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private GameState _gameState;
    private int _selectionIndex = 0;
    public FixturesViewScreen(SpriteFont font, GraphicsDeviceManager graphics, GameState gameState)
    {
        _font = font;
        _graphics = graphics;
        _gameState = gameState;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        int x = 100;
        int y = 100;
        
        spriteBatch.DrawString(_font, $"Matchday {_selectionIndex + 1}", new Vector2(x, y - 60), Color.White);
        foreach (Fixture fixture in _gameState.LeagueSelected.AllFixtures[_selectionIndex])
        {
            Color color1 = Color.White;
            Color color2 = Color.White;
            if(fixture.Team1 == _gameState.PlayerTeam){ color1 = Color.Cyan;}
            if(fixture.Team2 == _gameState.PlayerTeam){ color2 = Color.Cyan;}

            if(fixture.Completed == false){
                spriteBatch.DrawString(_font, $"{fixture.Team1.Name}", new Vector2(x, y), color1);
                spriteBatch.DrawString(_font, $"vs", new Vector2(x + 250, y), Color.White);
                spriteBatch.DrawString(_font, $"{fixture.Team2.Name}", new Vector2(x + 350, y), color2);
            }
            else{
                spriteBatch.DrawString(_font, $"{fixture.Team1.Name}", new Vector2(x, y), color1);
                spriteBatch.DrawString(_font, $"{fixture.Result[0]}:{fixture.Result[1]}", new Vector2(x + 250, y), Color.White);
                spriteBatch.DrawString(_font, $"{fixture.Team2.Name}", new Vector2(x + 350, y), color2);
            }
            y += 30;
        }


        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (inputState.IsKeyPressed(Keys.Left))
        {
            if (_selectionIndex == 0)
            {
                _selectionIndex = _gameState.LeagueSelected.AllFixtures.Count - 1;
            }
            else
            {
                _selectionIndex = Math.Max(0, _selectionIndex - 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Right))
        {
            if (_selectionIndex == _gameState.LeagueSelected.AllFixtures.Count - 1)
            {
                _selectionIndex = 0;
            }
            else
            {
                _selectionIndex = Math.Min(_gameState.LeagueSelected.AllFixtures.Count - 1, _selectionIndex + 1);
            }

        }

        if (inputState.IsKeyPressed(Keys.Enter))
        {
        }

        if (inputState.IsKeyPressed(Keys.Escape)){
            _gameState.LeagueSelected = null;
            ScreenManager.Instance.ChangeScreen("CareerMenu");
        }
    }
}