using System;
using System.Collections.Generic;
using FootballManagerGame.Enums;

namespace FootballManagerGame.Models;

public class Player
{
    public string Name { get; set; }
    
    public int Age { get; set; }
    public Dictionary<Attributes, int> Attributes { get; set; } = new Dictionary<Attributes, int>();
    public List<PlayerPositions> Positions { get; set; } = new List<PlayerPositions>();
    public int Overall { get; set; }


    public PlayerPositions PrimaryPosition => Positions.Count > 0 ? Positions[0] : PlayerPositions.CM;


    public bool CanPlayPosition(PlayerPositions position)
    {
        return Positions.Contains(position);
    }

    public void AddPosition(PlayerPositions position)
    {
        if (!Positions.Contains(position))
            Positions.Add(position);
    }
}