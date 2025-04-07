using System;
using System.Collections.Generic;
using FootballManagerGame.Enums;

namespace FootballManagerGame.Models;

public class Player{
    public string Name { get; set; }
    public DateTime Birthday { get; set; }
    public Dictionary<Attributes, int> Attributes { get; set; } = new Dictionary<Attributes, int>();
    public Dictionary<PlayerPositions, int> Positions { get; set; } = new Dictionary<PlayerPositions, int>();
    public int Overall { get; set; }
}