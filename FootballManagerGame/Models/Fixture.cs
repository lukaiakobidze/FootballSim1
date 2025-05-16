using System;
using System.Collections.Generic;
using FootballManagerGame.Data;


namespace FootballManagerGame.Models;

public class Fixture{

    public Team Team1 { get; set; }
    public Team Team2 { get; set; }
    public League League { get; set; }
    public DateTime Date { get; set; }
    public Team Winner { get; set; }
    public int FirstHalfTime { get; set; }
    public int SecondHalfTime { get; set; }
    public bool Completed { get; set; }
    public List<int> Result { get; set; } = new List<int>(){0,0};
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public int team1ChancesCreated { get; set; }
    public int team2ChancesCreated { get; set; }
    public int team1ShotAttempts { get; set; }
    public int team2ShotAttempts { get; set; }
    public int team1ShotOnTarget { get; set; }
    public int team2ShotOnTarget { get; set; }

    public Fixture() {
        Winner = null;
        Completed = false;
    }
        
    public Team GetWinner(){
        if(Winner != null){
            return Winner;
        }
        else{
            return null;
        }
    }
    public void AddGoal(Goal goal)
    {

        if (Team1.CurrentFormation.Players.ContainsValue(goal.PlayerScored))
        {
            Result[0] += 1;
            Goals.Add(goal);
        }
        else if (Team2.CurrentFormation.Players.ContainsValue(goal.PlayerScored))
        {
            Result[1] += 1;
            Goals.Add(goal);
        }
        else
        {
            throw new Exception("Scoring player is in neither team!");
        }
    }
    public void SetResult(int score1, int score2, List<Goal> goals){
        if (Completed != true){
            Result[0] = score1;
            Result[1] = score2;
            Goals = goals;
            Completed = true;
        }
        else{
            throw new Exception("Fixture already simulated");
        }
    }

    public void AddPoints()
    {
        if (Completed)
        {
            if (Result[0] > Result[1])
            {
                Winner = Team1;
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][1] += 1;
                League.Table[Team1.Name][4] += 3;
                League.Table[Team1.Name][5] += Result[0];
                League.Table[Team1.Name][6] += Result[1];
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][3] += 1;
                League.Table[Team2.Name][5] += Result[1];
                League.Table[Team2.Name][6] += Result[0];
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
            else if (Result[1] > Result[0])
            {
                Winner = Team2;
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][3] += 1;
                League.Table[Team1.Name][5] += Result[0];
                League.Table[Team1.Name][6] += Result[1];
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][1] += 1;
                League.Table[Team2.Name][4] += 3;
                League.Table[Team2.Name][5] += Result[1];
                League.Table[Team2.Name][6] += Result[0];
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
            else
            {
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][2] += 1;
                League.Table[Team1.Name][4] += 1;
                League.Table[Team1.Name][5] += Result[0];
                League.Table[Team1.Name][6] += Result[1];
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][2] += 1;
                League.Table[Team2.Name][4] += 1;
                League.Table[Team2.Name][5] += Result[1];
                League.Table[Team2.Name][6] += Result[0];
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
        }
        else
        {
            throw new Exception("Fixture is not completed!");
        }
        

    }
}
