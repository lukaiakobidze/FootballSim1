using System.Collections.Generic;
using FootballManagerGame.Data;


namespace FootballManagerGame.Models;

public class League{

    public string Name { get; set; }
    public int TeamAmount { get; set; } = 20;
    public int RelegationTeamAmount { get; set; }
    public int PromotionTeamAmount { get; set; }
    public string Nationality { get; set; }
    public List<Team> teams{ get; set; }
    public Dictionary<string, List<int>> Table { get; set; } = new Dictionary<string, List<int>>();

    public League(){
        teams = DataGenerator.GenerateLeague(TeamAmount);
        foreach (Team team in teams)
        {
            Table[team.Name] = new List<int>(){0, 0, 0, 0, 0, 0, 0, 0}; // 0.Games Played 1.Wins 2.Draws 3.Losses 4.Points 5. Goals For 6. Goals Against 7.+- Goals
        }
    }
}