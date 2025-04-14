using System;
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
    public List<List<Fixture>> AllFixtures { get; set; } = new List<List<Fixture>>{ };

    public League(){
        teams = DataGenerator.GenerateLeague(TeamAmount);
        foreach (Team team in teams)
        {
            Table[team.Name] = new List<int>(){0, 0, 0, 0, 0, 0, 0, 0}; // 0.Games Played 1.Wins 2.Draws 3.Losses 4.Points 5. Goals For 6. Goals Against 7.+- Goals
        }
           
        AllFixtures = GenerateFixtures(teams);

        foreach (var fixture in AllFixtures[0])
        {
            DataGenerator.SimFixture(fixture);
        }
    }

    public List<List<Fixture>> GenerateFixtures(List<Team> teams)
    {
        int teamCount = teams.Count;
        if (teamCount % 2 != 0)
            throw new ArgumentException("Team count must be even.");

        int rounds = teamCount - 1;
        int half = teamCount / 2;

        // Create a copy so we don’t modify the original list
        List<Team> rotation = new List<Team>(teams);

        // Fix the last team
        Team fixedTeam = rotation[teamCount - 1];
        rotation.RemoveAt(teamCount - 1); // Remove fixed team for rotation

        List<List<Fixture>> allMatchdays = new List<List<Fixture>>();

        // First leg
        for (int round = 0; round < rounds; round++)
        {
            List<Fixture> matchday = new List<Fixture>();

            // Insert fixed team into appropriate position (acts like "last" team)
            List<Team> roundTeams = new List<Team>(rotation);
            roundTeams.Insert(half, fixedTeam);

            for (int i = 0; i < half; i++)
            {
                Team home = roundTeams[i];
                Team away = roundTeams[teamCount - 1 - i];

                matchday.Add(new Fixture(){Team1 = home, Team2 = away, League = this});
            }
            DataGenerator.Shuffle(matchday);
            allMatchdays.Add(matchday);

            // Rotate clockwise: keep first, move last to second, shift rest right
            Team last = rotation[rotation.Count - 1];
            rotation.RemoveAt(rotation.Count - 1);
            rotation.Insert(1, last);
        }

        // Second leg: reverse home/away
        List<List<Fixture>> returnLegs = new List<List<Fixture>>();
        foreach (var matchday in allMatchdays)
        {
            List<Fixture> reversedMatchday = new List<Fixture>();
            foreach (var match in matchday)
            {
                reversedMatchday.Add(new Fixture(){Team1 = match.Team2, Team2 = match.Team1, League = this});
            }
            returnLegs.Add(reversedMatchday);
        }

        allMatchdays.AddRange(returnLegs);
        return allMatchdays;
    }

}