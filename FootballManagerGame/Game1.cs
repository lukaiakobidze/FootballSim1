using System;
using FootballManagerGame.Input;
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
    public static bool ExitGame = false;
    private InputState _inputState;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _inputState = new InputState();
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.IsFullScreen = true;        
    }


    protected override void Initialize()
    {

        _gameState = new GameState
        {
            CurrentDate = new DateTime(2024, 8, 1)
        };

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("GameFont");

        ScreenManager.Instance.AddScreen("MainMenu", new MainMenuScreen(_font));
        //ScreenManager.Instance.AddScreen("TeamView", new TeamViewScreen(_gameState, _font));
        //ScreenManager.Instance.AddScreen("NewGame", new NewGameScreen(_gameState, _font));
        //ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font));
        ScreenManager.Instance.ChangeScreen("MainMenu");
    }


    protected override void Update(GameTime gameTime)
    {
        if (ExitGame)
            Exit();

        
        _inputState.Update();


        ScreenManager.Instance.CurrentScreen?.HandleInput(_inputState);
        ScreenManager.Instance.Update(gameTime);
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
