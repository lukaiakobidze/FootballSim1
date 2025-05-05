using FootballManagerGame.Enums;
using FootballManagerGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FootballManagerGame.Helpers;

public class FormationDrawer
{
    public static Dictionary<PlayerPositions, Vector2> PositionCords = new Dictionary<PlayerPositions, Vector2>{
        {PlayerPositions.GK, new Vector2(300,100)},
        {PlayerPositions.CB, new Vector2(300,200)},
        {PlayerPositions.LCB, new Vector2(200,200)},
        {PlayerPositions.RCB, new Vector2(400,200)},
        {PlayerPositions.LB, new Vector2(100,220)},
        {PlayerPositions.LWB, new Vector2(100,300)},
        {PlayerPositions.RB, new Vector2(500,220)},
        {PlayerPositions.RWB, new Vector2(500,300)},
        {PlayerPositions.CDM, new Vector2(300,300)},
        {PlayerPositions.LDM, new Vector2(200,300)},
        {PlayerPositions.RDM, new Vector2(400,300)},
        {PlayerPositions.CM, new Vector2(300,400)},
        {PlayerPositions.LCM, new Vector2(200,400)},
        {PlayerPositions.RCM, new Vector2(400,400)},
        {PlayerPositions.CAM, new Vector2(300,500)},
        {PlayerPositions.LAM, new Vector2(200,500)},
        {PlayerPositions.RAM, new Vector2(400,500)},
        {PlayerPositions.LM, new Vector2(100,450)},
        {PlayerPositions.RM, new Vector2(500,450)},
        {PlayerPositions.LW, new Vector2(140,600)},
        {PlayerPositions.RW, new Vector2(460,600)},
        {PlayerPositions.F9, new Vector2(300,550)},
        {PlayerPositions.LF, new Vector2(200,650)},
        {PlayerPositions.RF, new Vector2(400,650)},
        {PlayerPositions.CF, new Vector2(300,660)} };

    private static Color txtColor = new Color(5, 7, 18);

    public static void DrawFormation(Formation formation, Vector2 origin, float size, SpriteBatch spriteBatch, List<Texture2D> textures, GraphicsDeviceManager graphics, 
        SpriteFont font, GameState gameState, int index)
    {

        Rectangle area = new Rectangle((int)(origin.X), (int)(origin.Y), (int)(600*size), (int)(800*size));

        spriteBatch.Draw(textures[1], area, new Color(150, 150, 255, 50));
        
        
        for (int i = 0; i < formation.Positions.Count; i++)
        {
            Color colorOVR = Color.White;

            if (i == index){
                colorOVR = Color.Yellow;
            }

            spriteBatch.Draw(textures[0], new Rectangle((int)(area.X + PositionCords[formation.Positions[i]].X - 45 * size), 
                (int)(area.Y + area.Height - PositionCords[formation.Positions[i]].Y - 45 * size), (int)( 90 * size), (int)( 90 * size)), colorOVR);
            
        

            spriteBatch.DrawString(font, $"{formation.Positions[i]}", new Vector2((int)(area.X + PositionCords[formation.Positions[i]].X), 
                (int)(area.Y + area.Height - PositionCords[formation.Positions[i]].Y)), txtColor, 0f, 
                font.MeasureString($"{formation.Positions[i]}") / 2, 0.85f * size, SpriteEffects.None, 0f);

            if(gameState.TeamSelected.CurrentFormation.Players.ContainsKey(formation.Positions[i])){

                if (gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall < 40) { colorOVR = Color.IndianRed; }
                        else if (gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall < 60) { colorOVR = Color.Orange; }
                        else if (gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall < 70) { colorOVR = Color.Yellow; }
                        else if (gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall < 80) { colorOVR = Color.LightGreen; }
                        else if (gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall < 90) { colorOVR = Color.SpringGreen; }
                        else { colorOVR = Color.Cyan; }


                spriteBatch.DrawString(font, $"{gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1]}", 
                new Vector2((int)(area.X + PositionCords[formation.Positions[i]].X), (int)(area.Y + area.Height - PositionCords[formation.Positions[i]].Y + 40)), Color.White, 0f, 
                font.MeasureString($"{gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1]}") / 2, 
                0.85f * size, SpriteEffects.None, 0f);

                spriteBatch.DrawString(font, $"{gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall}", 
                new Vector2((int)(area.X + PositionCords[formation.Positions[i]].X), (int)(area.Y + area.Height - PositionCords[formation.Positions[i]].Y + 55)), colorOVR, 0f, 
                font.MeasureString($"{gameState.TeamSelected.CurrentFormation.Players[formation.Positions[i]].LiveOverall}") / 2, 0.85f * size, 
                SpriteEffects.None, 0f);
            }
            
            
            

        }
    }
}

