using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FootballManagerGame.Helpers;
public class ShapeDrawer
{
    private readonly Texture2D _pixel;
    private readonly SpriteBatch _spriteBatch;

    public ShapeDrawer(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;

        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        Vector2 edge = end - start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);

        _spriteBatch.Draw(_pixel,
            new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), (int)thickness),
            null,
            color,
            angle,
            Vector2.Zero,
            SpriteEffects.None,
            0);
    }

    public void DrawRect(Rectangle rect, Color color)
    {
        // Top
        DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), color);
        // Right
        DrawLine(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), color);
        // Bottom
        DrawLine(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom), color);
        // Left
        DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top), color);
    }

    public void DrawCircle(Vector2 center, float radius, int sides, Color color)
    {
        float increment = MathF.Tau / sides;
        Vector2 lastPoint = center + radius * new Vector2(MathF.Cos(0), MathF.Sin(0));

        for (int i = 1; i <= sides; i++)
        {
            float angle = i * increment;
            Vector2 nextPoint = center + radius * new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            DrawLine(lastPoint, nextPoint, color);
            lastPoint = nextPoint;
        }
    }
}
