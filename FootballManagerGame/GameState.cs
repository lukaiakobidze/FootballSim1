using System;
using System.Collections.Generic;
using FootballManagerGame.Data;
using FootballManagerGame.Models;
using Microsoft.Xna.Framework;

namespace FootballManagerGame;


public class GameState
{
    public GameState()
    {
        Leagues = new List<League>();
    }
    
    public Team PlayerTeam { get; set; }
    public League PlayerLeague { get; set; }
    public List<League> Leagues { get; set; }
    public DateTime CurrentDate { get; set; }
    public Player PlayerSelected { get; set; }
    public Team TeamSelected { get; set; }
    public League LeagueSelected { get; set; }
    public int SaveSlot { get; set; }

    public void InitializeNewGame(int saveSlot)
    {
        SaveSlot = saveSlot;
        Leagues.Add(new League()
        {
            Name = "League 1",
            TeamAmount = 20,
            SeasonStart = new DateTime(2024, 8, 17)
        });
        Leagues[0].InitializeNewLeague();
        PlayerLeague = Leagues[0];
        CurrentDate = new DateTime(2024, 8, 1);
    }
}