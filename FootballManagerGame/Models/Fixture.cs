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
    public bool ExtraTime { get; set; }
    public bool Completed { get; set; }
    public List<int> Result { get; set; } = new List<int>();

    public Fixture() {
        Winner = null;
        ExtraTime = false;
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

    public void SetResult(int score1, int score2){
        if (Completed != true){
            Result.Add(score1);
            Result.Add(score2);
            Completed = true;
            if (score1 > score2){
                Winner = Team1;
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][1] += 1;
                League.Table[Team1.Name][4] += 3;
                League.Table[Team1.Name][5] += score1;
                League.Table[Team1.Name][6] += score2;
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][3] += 1;
                League.Table[Team2.Name][5] += score2;
                League.Table[Team2.Name][6] += score1;
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
            else if (score2 > score1){
                Winner = Team2;
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][3] += 1;
                League.Table[Team1.Name][5] += score1;
                League.Table[Team1.Name][6] += score2;
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][1] += 1;
                League.Table[Team2.Name][4] += 3;
                League.Table[Team2.Name][5] += score2;
                League.Table[Team2.Name][6] += score1;
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
            else{
                League.Table[Team1.Name][0] += 1;
                League.Table[Team1.Name][2] += 1;
                League.Table[Team1.Name][4] += 1;
                League.Table[Team1.Name][5] += score1;
                League.Table[Team1.Name][6] += score2;
                League.Table[Team1.Name][7] = League.Table[Team1.Name][5] - League.Table[Team1.Name][6];
                League.Table[Team2.Name][0] += 1;
                League.Table[Team2.Name][2] += 1;
                League.Table[Team2.Name][4] += 1;
                League.Table[Team2.Name][5] += score2;
                League.Table[Team2.Name][6] += score1;
                League.Table[Team2.Name][7] = League.Table[Team2.Name][5] - League.Table[Team2.Name][6];
            }
        }
        else{
            throw new Exception("Fixture already simulated");
        }
    }

    
}
