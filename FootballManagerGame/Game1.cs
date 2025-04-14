using System;
using FootballManagerGame.Data;
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
    private SpriteFont _font;
    public static bool ExitGame = false;
    private InputState _inputState;
    private GameDataService _gameDataService;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _inputState = new InputState();
        _graphics.PreferredBackBufferWidth = 1440;
        _graphics.PreferredBackBufferHeight = 900;
        _graphics.IsFullScreen = false;
    }


    protected override void Initialize()
    {

        _gameDataService = new GameDataService();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("GameFont");

        ScreenManager.Instance.AddScreen("MainMenu", new MainMenuScreen(_font, _graphics, _gameDataService));
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
        GraphicsDevice.Clear(Color.Black);
        ScreenManager.Instance.Draw(_spriteBatch);


        base.Draw(gameTime);
    }
}
