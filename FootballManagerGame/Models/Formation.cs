using System;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Enums;


namespace FootballManagerGame.Models;

public class Formation{

    public List<PlayerPositions> Positions { get; set;}
    public Dictionary<PlayerPositions, Player> Players { get; set; }
    public virtual string Name => "Custom";
    public Formation(){
        Positions = new List<PlayerPositions>();
        Players = new Dictionary<PlayerPositions, Player>();
        
    }

    public void AssignPlayerToPosition(Player player, PlayerPositions position){

            if (Positions.Contains(position))
            {
                Players[position] = player;
            }
            else
            {
                throw new ArgumentException($"Position {position} not in formation.");
            }
        }

    public void RemovePlayers(){
        Players = new Dictionary<PlayerPositions, Player>();
    }

    public bool IsComplete() => Players.Count == Positions.Count;
}

public class FourFourTwoFormation : Formation{
    public override string Name => "442";

    public FourFourTwoFormation()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.LM,
            PlayerPositions.LCM,
            PlayerPositions.RCM,
            PlayerPositions.RM,
            PlayerPositions.LF,
            PlayerPositions.RF
        };
    }
}

public class FourThreeThreeFormation : Formation{
    public override string Name => "433";

    public FourThreeThreeFormation()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.LCM,
            PlayerPositions.CM,
            PlayerPositions.RCM,
            PlayerPositions.LW,
            PlayerPositions.ST,
            PlayerPositions.RW
        };
    }
}