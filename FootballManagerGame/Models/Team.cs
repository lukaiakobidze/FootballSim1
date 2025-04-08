using System.Collections.Generic;

namespace FootballManagerGame.Models;

public class Team
{

    public string Name { get; set; }
    public string NameShort { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
}