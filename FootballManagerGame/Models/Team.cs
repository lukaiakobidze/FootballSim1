using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Enums;

namespace FootballManagerGame.Models;

public class Team
{

    public string Name { get; set; }
    public string NameShort { get; set; }
    public string Stadium { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public int AvgOvr { get; set; }
    public Formation CurrentFormation { get; set; } = new FourFourTwoFormation();

    public void CalcAvg(){
        
        int i = 0;
        int sum = 0;
        foreach (var pos in CurrentFormation.Positions){
            if (CurrentFormation.Players.ContainsKey(pos)){
                sum += CurrentFormation.Players[pos].LiveOverall;
                i++;
            }
            else{
                i++;
            }
        }
        AvgOvr = (int)(sum / i);
    }
}