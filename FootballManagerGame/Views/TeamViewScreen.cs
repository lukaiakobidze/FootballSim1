using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FootballManagerGame.Input;
using FootballManagerGame.Models;
namespace FootballManagerGame.Views;


public class TeamViewScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    
    public TeamViewScreen(GameState gameState, SpriteFont font)
    {
        _gameState = gameState;
        _font = font;
    }
    
    public override void Update(GameTime gameTime)
    {
        // Add update logic
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Team: " + _gameState.PlayerTeam?.Name ?? "No Team Selected", 
            new Vector2(100, 100), Color.White);
            
        // Draw player list if team exists
        if (_gameState.PlayerTeam != null)
        {
            int y = 150;
            foreach (var player in _gameState.PlayerTeam.Players)
            {
                spriteBatch.DrawString(_font, 
                    $"{player.Name}", 
                    new Vector2(100, y), Color.White);
                y += 30;
            }
        }
        
        spriteBatch.End();
    }
    
    public override void HandleInput(InputState inputState)
    {
        // Handle team view input
    }
}