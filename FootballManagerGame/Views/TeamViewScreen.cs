using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
using System.Collections.Generic;
namespace FootballManagerGame.Views;


public class TeamViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private int _selectedPlayerIndex = 0;
    private string _origin;
    public TeamViewScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics, string origin)
    {
        _graphics = graphics;
        _gameState = gameState;
        _font = font;
        _origin = origin;
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Team: " + _gameState.TeamSelected?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);

        Color colorAVG = Color.White;
        if (_gameState.TeamSelected.AvgAttack < 40) { colorAVG = Color.IndianRed; }
        else if (_gameState.TeamSelected.AvgAttack < 60) { colorAVG = Color.Orange; }
        else if (_gameState.TeamSelected.AvgAttack < 70) { colorAVG = Color.Yellow; }
        else if (_gameState.TeamSelected.AvgAttack < 80) { colorAVG = Color.LightGreen; }
        else if (_gameState.TeamSelected.AvgAttack < 90) { colorAVG = Color.SpringGreen; }
        else { colorAVG = Color.Cyan; }
        spriteBatch.DrawString(_font, $"Attack: {_gameState.TeamSelected.AvgAttack}", new Vector2(500, 50), colorAVG);

        if (_gameState.TeamSelected.AvgMidfield < 40) { colorAVG = Color.IndianRed; }
        else if (_gameState.TeamSelected.AvgMidfield < 60) { colorAVG = Color.Orange; }
        else if (_gameState.TeamSelected.AvgMidfield < 70) { colorAVG = Color.Yellow; }
        else if (_gameState.TeamSelected.AvgMidfield < 80) { colorAVG = Color.LightGreen; }
        else if (_gameState.TeamSelected.AvgMidfield < 90) { colorAVG = Color.SpringGreen; }
        else { colorAVG = Color.Cyan; }
        spriteBatch.DrawString(_font, $"Midfield: {_gameState.TeamSelected.AvgMidfield}", new Vector2(700, 50), colorAVG);

        if (_gameState.TeamSelected.AvgDefence < 40) { colorAVG = Color.IndianRed; }
        else if (_gameState.TeamSelected.AvgDefence < 60) { colorAVG = Color.Orange; }
        else if (_gameState.TeamSelected.AvgDefence < 70) { colorAVG = Color.Yellow; }
        else if (_gameState.TeamSelected.AvgDefence < 80) { colorAVG = Color.LightGreen; }
        else if (_gameState.TeamSelected.AvgDefence < 90) { colorAVG = Color.SpringGreen; }
        else { colorAVG = Color.Cyan; }
        spriteBatch.DrawString(_font, $"Defence: {_gameState.TeamSelected.AvgDefence}", new Vector2(900, 50), colorAVG);


        var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ToList();
        if (_gameState.TeamSelected != null)
        {
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
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
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
        if (inputState.IsKeyPressed(Keys.Q))
        {
            var orderedList = _gameState.TeamSelected.Players.OrderBy(p => p.Positions.First()).ToList();
            _gameState.PlayerSelected = orderedList[_selectedPlayerIndex];
            ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, "TeamView"));
            ScreenManager.Instance.ChangeScreen("PlayerView");

        }
        if (inputState.IsKeyPressed(Keys.Escape))
        {
            _gameState.TeamSelected = null;
            if(_origin == "TableView"){
                ScreenManager.Instance.ChangeScreen("TableView");
            }
            else if (_origin == "CareerMenu"){
                ScreenManager.Instance.ChangeScreen("CareerMenu");
            }
            
        }
    }
}