using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FootballManagerGame.Input;

namespace FootballManagerGame.Views;

public class MainMenuScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font; // You'll need to add a font to your Content project
    
    public MainMenuScreen(GameState gameState, SpriteFont font)
    {
        _gameState = gameState;
        _font = font;
    }
    
    public override void Update(GameTime gameTime)
    {
        // Add update logic here
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, "Football Manager Game", new Vector2(100, 100), Color.White);
        spriteBatch.DrawString(_font, "1. New Game", new Vector2(100, 150), Color.White);
        spriteBatch.DrawString(_font, "2. Exit", new Vector2(100, 200), Color.White);
        spriteBatch.End();
    }
    
    public override void HandleInput(InputState inputState)
    {
        // Handle menu selection
    }
}