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

    public void RemoveAllPlayers(){
        Players = new Dictionary<PlayerPositions, Player>();
    }

    public void RemovePlayer(PlayerPositions position){
        Players.Remove(position);
    }

    public bool IsComplete() => Players.Count == Positions.Count;

    public virtual void InitPositions(){}
}

public class FourFourTwoFormation : Formation{
    public override string Name => "4-4-2";

    public override void InitPositions()
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
public class FourFourOneOneFormation : Formation{
    public override string Name => "4-4-1-1";

    public override void InitPositions(){
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
            PlayerPositions.F9,
            PlayerPositions.CF
        };
    }
}

public class FourThreeThreeFormation : Formation{
    public override string Name => "4-3-3";

    public override void InitPositions()
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
            PlayerPositions.CF,
            PlayerPositions.RW
        };
    }
}

public class FourThreeThreeAttackingFormation : Formation{
    public override string Name => "4-3-3 Attacking";
    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.LCM,
            PlayerPositions.CAM,
            PlayerPositions.RCM,
            PlayerPositions.LW,
            PlayerPositions.CF,
            PlayerPositions.RW
        };
    }
}
public class FourThreeThreeDefensiveFormation : Formation{
    public override string Name => "4-3-3 Defensive";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.LCM,
            PlayerPositions.CDM,
            PlayerPositions.RCM,
            PlayerPositions.LW,
            PlayerPositions.CF,
            PlayerPositions.RW
        };
    }
}
public class FourTwoOneTwoOneFormation : Formation{
    public override string Name => "4-2-1-2-1";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.LDM,
            PlayerPositions.RDM,
            PlayerPositions.CM,
            PlayerPositions.LAM,
            PlayerPositions.RAM,
            PlayerPositions.CF
        };
    }
}
public class FourOneTwoOneTwoFormation : Formation{
    public override string Name => "4-1-2-1-2";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LCB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.CDM,
            PlayerPositions.LCM,
            PlayerPositions.RCM,
            PlayerPositions.CAM,
            PlayerPositions.LF,
            PlayerPositions.RF
        };
    }
}
public class FourFiveOneFormation : Formation{
    public override string Name => "4-5-1";

    public override void InitPositions()
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
            PlayerPositions.CM,
            PlayerPositions.RCM,
            PlayerPositions.RM,
            PlayerPositions.CF
        };
    }
}
public class FiveThreeTwoFormation : Formation{
    public override string Name => "5-3-2";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LWB,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.RWB,
            PlayerPositions.LCM,
            PlayerPositions.CDM,
            PlayerPositions.RCM,
            PlayerPositions.LF,
            PlayerPositions.RF
        };
    }
}

public class FiveTwoOneTwoFormation : Formation{
    public override string Name => "5-2-1-2";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LWB,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.RWB,
            PlayerPositions.LDM,
            PlayerPositions.RDM,
            PlayerPositions.CAM,
            PlayerPositions.LF,
            PlayerPositions.RF
        };
    }
}

public class FiveFourOneFormation : Formation{
    public override string Name => "5-4-1";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LWB,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.RWB,
            PlayerPositions.LM,
            PlayerPositions.LCM,
            PlayerPositions.RCM,
            PlayerPositions.RM,
            PlayerPositions.CF
        };
    }
}

public class ThreeFourThreeFormation : Formation{
    public override string Name => "3-4-3";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.LM,
            PlayerPositions.LCM,
            PlayerPositions.RCM,
            PlayerPositions.RM,
            PlayerPositions.LW,
            PlayerPositions.RW,
            PlayerPositions.CF
        };
    }
}
public class ThreeFourOneTwoFormation : Formation{
    public override string Name => "3-4-1-2";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.LM,
            PlayerPositions.LCM,
            PlayerPositions.RCM,
            PlayerPositions.RM,
            PlayerPositions.CAM,
            PlayerPositions.LF,
            PlayerPositions.RF
        };
    }
}

public class AllPosFormation : Formation{
    public override string Name => "111";

    public override void InitPositions()
    {
        Positions = new List<PlayerPositions>
        {
            PlayerPositions.GK,
            PlayerPositions.LB,
            PlayerPositions.LWB,
            PlayerPositions.LCB,
            PlayerPositions.CB,
            PlayerPositions.RCB,
            PlayerPositions.RB,
            PlayerPositions.RWB,
            PlayerPositions.CDM,
            PlayerPositions.LDM,
            PlayerPositions.RDM,
            PlayerPositions.LCM,
            PlayerPositions.CM,
            PlayerPositions.RCM,
            PlayerPositions.CAM,
            PlayerPositions.LAM,
            PlayerPositions.RAM,
            PlayerPositions.LM,
            PlayerPositions.RM,
            PlayerPositions.LW,
            PlayerPositions.F9,
            PlayerPositions.LF,
            PlayerPositions.RF,
            PlayerPositions.CF,
            PlayerPositions.RW
        };
    }
}