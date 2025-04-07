using Microsoft.Xna.Framework.Input;

namespace FootballManagerGame.Input;

public class InputState
{
    public KeyboardState CurrentKeyboardState { get; private set; }
    public KeyboardState PreviousKeyboardState { get; private set; }
    public MouseState CurrentMouseState { get; private set; }
    public MouseState PreviousMouseState { get; private set; }
    
    public void Update()
    {
        PreviousKeyboardState = CurrentKeyboardState;
        CurrentKeyboardState = Keyboard.GetState();
        
        PreviousMouseState = CurrentMouseState;
        CurrentMouseState = Mouse.GetState();
    }
    
    public bool IsKeyPressed(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
    }
    
    public bool IsLeftMousePressed()
    {
        return CurrentMouseState.LeftButton == ButtonState.Pressed && 
               PreviousMouseState.LeftButton == ButtonState.Released;
    }
}