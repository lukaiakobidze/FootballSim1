using System;
using System.Collections.Generic;
using FootballManagerGame.Models;

namespace FootballManagerGame;

public class GameState
{
    public Team PlayerTeam { get; set; }
    public List<Team> AllTeams { get; set; }
    public DateTime CurrentDate { get; set; }
    public Player PlayerSelected { get; set; }


}