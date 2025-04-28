using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
using System.Collections.Generic;
using FootballManagerGame.Helpers;
namespace FootballManagerGame.Views;


public class TeamStrategyScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private List<Texture2D> _textures;
    private int _selectionIndex = 0;
    private int _selectedPlayerIndex = 0;
    private bool _showPlayers = false;
    private List<string> _strings;
    

    public TeamStrategyScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _graphics = graphics;
        _gameState = gameState;
        _font = font;
        _shapes = shapes;
        _textures = textures;   
        _strings = new List<string>() {"Change formation", "Change players"};
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        if (_gameState.TeamSelected != null)
        {
            spriteBatch.DrawString(_font, "Team: " + _gameState.TeamSelected?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);

            if(_showPlayers){
                var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ThenBy(p => p.Overall).ToList();
                int y = 130;
                for (int i = 0; i < orderedList.Count; i++)
                {
                    Color color = (i == _selectedPlayerIndex) ? Color.Yellow : Color.White;
                    Color colorOVR = Color.White;
                    string positions = string.Join("/", orderedList[i].Positions);
                    spriteBatch.DrawString(_font, $"{orderedList[i].Name} - {positions} - Age: {orderedList[i].Age}", new Vector2(100, y), color);

                    if (orderedList[i].Overall < 40) { colorOVR = Color.IndianRed; }
                    else if (orderedList[i].Overall < 60) { colorOVR = Color.Orange; }
                    else if (orderedList[i].Overall < 70) { colorOVR = Color.Yellow; }
                    else if (orderedList[i].Overall < 80) { colorOVR = Color.LightGreen; }
                    else if (orderedList[i].Overall < 90) { colorOVR = Color.SpringGreen; }
                    else { colorOVR = Color.Cyan; }
                    spriteBatch.DrawString(_font, $"{orderedList[i].Overall}", new Vector2(500, y), colorOVR);
                    y += 30;
                }
            }
            else
            {
                for (int i = 0; i < _strings.Count; i++)
                {
                    Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
                    spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 100 + i * 30), color);
                }
            }
            



            FormationDrawer.DrawFormation(_gameState.TeamSelected.CurrentFormation.Name, new Rectangle(_graphics.GraphicsDevice.Viewport.Width-600, 100, 500, 700), spriteBatch, _textures, _graphics, _font);





        }

        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if(_showPlayers == false){

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
                if(_selectionIndex == 0){

                }
                else if (_selectionIndex == 1){
                    _showPlayers = true;
                }
            }

            if (inputState.IsKeyPressed(Keys.Escape))
            {
                _gameState.TeamSelected = null;
                ScreenManager.Instance.ChangeScreen("CareerMenu");
            }

        }
        else{

            if (inputState.IsKeyPressed(Keys.Up))
            {
                if (_selectedPlayerIndex == 0)
                {
                    _selectedPlayerIndex = _gameState.TeamSelected.Players.Count - 1;
                }
                else{
                    _selectedPlayerIndex = Math.Max(0, _selectedPlayerIndex - 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Down))
            {
                if (_selectedPlayerIndex == _gameState.TeamSelected.Players.Count - 1)
                {
                    _selectedPlayerIndex = 0;
                }
                else
                {
                    _selectedPlayerIndex = Math.Min(_gameState.TeamSelected.Players.Count - 1, _selectedPlayerIndex + 1);
                }
            }
            if (inputState.IsKeyPressed(Keys.Enter))
            {
                var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ToList();
                _gameState.PlayerSelected = orderedList[_selectedPlayerIndex];
                ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, orderedList[_selectedPlayerIndex], "TeamStrategyView"));
                ScreenManager.Instance.ChangeScreen("PlayerView");

            }
            if (inputState.IsKeyPressed(Keys.Escape)){
                _showPlayers = false;
            }
        }

        
    }
}