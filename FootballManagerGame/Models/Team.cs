using System.Collections.Generic;
using FootballManagerGame.Enums;

namespace FootballManagerGame.Models;

public class Team
{

    public string Name { get; set; }
    public string NameShort { get; set; }
    public string Stadium { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public int AvgAttack { get; set; }
    public int AvgMidfield { get; set; }
    public int AvgDefence { get; set; }
    public Formation CurrentFormation { get; set; } = new FourFourTwoFormation();

    public void CalcAvg(){
        float att = 0;
        float mid = 0;
        float def = 0;
        int i = 0;
        
        foreach (Player player in Players){
            if (player.PrimaryPosition == PlayerPositions.GK || player.PrimaryPosition == PlayerPositions.CB || player.PrimaryPosition == PlayerPositions.LB || player.PrimaryPosition == PlayerPositions.RB || player.PrimaryPosition == PlayerPositions.LWB || player.PrimaryPosition == PlayerPositions.RWB){
                def += player.Overall;
                i++;
            }
        }
        AvgDefence = (int)(def / i);

        i = 0;
        foreach (Player player in Players){
            if (player.PrimaryPosition == PlayerPositions.CDM || player.PrimaryPosition == PlayerPositions.CM || player.PrimaryPosition == PlayerPositions.CAM || player.PrimaryPosition == PlayerPositions.LM || player.PrimaryPosition == PlayerPositions.RM){
                mid += player.Overall;
                i++;
            }
        }
        AvgMidfield = (int)(mid / i);

        i = 0;
        foreach (Player player in Players){
            if (player.PrimaryPosition == PlayerPositions.LW || player.PrimaryPosition == PlayerPositions.RW || player.PrimaryPosition == PlayerPositions.LF || player.PrimaryPosition == PlayerPositions.RF || player.PrimaryPosition == PlayerPositions.ST || player.PrimaryPosition == PlayerPositions.CF){
                att += player.Overall;
                i++;
            }
        }
        AvgAttack = (int)(att / i);
    }
}