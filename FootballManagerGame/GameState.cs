using System;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;

namespace FootballManagerGame;


public class GameState
{
    public GameState()
    {
        AllTeams = new List<Team>();
    }

    public GameState(int saveSlot) : this()
    {
        SaveSlot = saveSlot;
        AllTeams = DataGenerator.GenerateLeague("League 1", 20);
    }
    
    public Team PlayerTeam { get; set; }
    public List<Team> AllTeams { get; set; }
    public DateTime CurrentDate { get; set; }
    public Player PlayerSelected { get; set; }
    public Team TeamSelected { get; set; }
    public int SaveSlot { get; set; }
}