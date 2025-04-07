using System;
using FootballManagerGame.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FootballManagerGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _gameState;
    private SpriteFont _font;
    //private ScreenManager _screenManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        
        _gameState = new GameState
        {
            CurrentDate = new DateTime(2023, 7, 1), // Starting date for your game
            PlayerTeam = new Models.Team{Name = "PSG"}
        };

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("GameFont");

        ScreenManager.Instance.AddScreen("MainMenu", new MainMenuScreen(_gameState, _font));
        ScreenManager.Instance.ChangeScreen("MainMenu");
    }
    

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        ScreenManager.Instance.Draw(_spriteBatch);
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
