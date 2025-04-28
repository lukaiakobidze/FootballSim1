using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FootballManagerGame.Helpers;

public class FormationDrawer
{
    public static void DrawFormation(string formation, Rectangle area, SpriteBatch spriteBatch, List<Texture2D> textures, GraphicsDeviceManager graphics, SpriteFont font)
    {
        spriteBatch.Draw(textures[1], area, Color.White);
        
        if (formation == "442")
        {
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 250 - 32, area.Y + area.Height - 100 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"GK", new Vector2(area.X + 250, area.Y + area.Height - 60), Color.White, 0f, font.MeasureString("GK") / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(textures[0], new Rectangle(area.X + 100 - 32, area.Y + area.Height - 200 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"LB", new Vector2(area.X + 100, area.Y + area.Height - 160), Color.White, 0f, font.MeasureString("LB") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 200 - 32, area.Y + area.Height - 200 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"LCB", new Vector2(area.X + 200, area.Y + area.Height - 160), Color.White, 0f, font.MeasureString("LCB") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 300 - 32, area.Y + area.Height - 200 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"RCB", new Vector2(area.X + 300, area.Y + area.Height - 160), Color.White, 0f, font.MeasureString("RCB") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 400 - 32, area.Y + area.Height - 200 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"RB", new Vector2(area.X + 400, area.Y + area.Height - 160), Color.White, 0f, font.MeasureString("RB") / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(textures[0], new Rectangle(area.X + 100 - 32, area.Y + area.Height - 350 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"LM", new Vector2(area.X + 100, area.Y + area.Height - 310), Color.White, 0f, font.MeasureString("LM") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 200 - 32, area.Y + area.Height - 350 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"LCM", new Vector2(area.X + 200, area.Y + area.Height - 310), Color.White, 0f, font.MeasureString("LCM") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 300 - 32, area.Y + area.Height - 350 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"RCM", new Vector2(area.X + 300, area.Y + area.Height - 310), Color.White, 0f, font.MeasureString("RCM") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 400 - 32, area.Y + area.Height - 350 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"RM", new Vector2(area.X + 400, area.Y + area.Height - 310), Color.White, 0f, font.MeasureString("RM") / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(textures[0], new Rectangle(area.X + 200 - 32, area.Y + area.Height - 500 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"LF", new Vector2(area.X + 200, area.Y + area.Height - 460), Color.White, 0f, font.MeasureString("LF") / 2, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(textures[0], new Rectangle(area.X + 300 - 32, area.Y + area.Height - 500 - 32, 64, 64), Color.White);
            spriteBatch.DrawString(font, $"RF", new Vector2(area.X + 300, area.Y + area.Height - 460), Color.White, 0f, font.MeasureString("RF") / 2, 1f, SpriteEffects.None, 0f);
        }

    }



}