using System;
using System.Collections.Generic;
using FootballManagerGame.Models;

namespace FootballManagerGame;

public class GameState{
    public Team PlayerTeam { get; set; }
    public List<Team> AllTeams { get; set; } = new List<Team>();
    public DateTime CurrentDate { get; set; }

    
}