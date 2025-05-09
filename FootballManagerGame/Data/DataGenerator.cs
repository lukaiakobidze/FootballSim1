using System;
using System.Collections.Generic;
using System.Linq;
using FootballManagerGame.Enums;
using FootballManagerGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FootballManagerGame.Data;

public class DataGenerator
{
    private static Random _random = new Random();

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static void SimFixture(Fixture fixture){
        double team1Strength = Math.Pow(fixture.Team1.AvgAttack * 0.33 + fixture.Team1.AvgMidfield * 0.33 + fixture.Team1.AvgDefence * 0.33, 3);
        double team2Strength = Math.Pow(fixture.Team2.AvgAttack * 0.33 + fixture.Team2.AvgMidfield * 0.33 + fixture.Team2.AvgDefence * 0.33, 3);


        double ratio = team1Strength / (team1Strength + team2Strength);

        int maxGoals = 8;
        int team1Score = _random.Next((int)(ratio * maxGoals) + 1);
        int team2Score = _random.Next((int)((1 - ratio) * maxGoals) + 1);
        fixture.SetResult(team1Score, team2Score);
    }
    public static Team GenerateTeam(string name, string nameShort)
    {
        Team team = new Team
        {
            Name = name,
            NameShort = nameShort,
            Players = new List<Player>()
        };


        int playerCount = _random.Next(7, 12);
        team.Players.Add(GeneratePlayer(PlayerPositions.GK));
        team.Players.Add(GeneratePlayer(PlayerPositions.CB));
        team.Players.Add(GeneratePlayer(PlayerPositions.CB));
        team.Players.Add(GeneratePlayer(PlayerPositions.LB));
        team.Players.Add(GeneratePlayer(PlayerPositions.RB));
        team.Players.Add(GeneratePlayer(PlayerPositions.CM));
        team.Players.Add(GeneratePlayer(PlayerPositions.CDM));
        team.Players.Add(GeneratePlayer(PlayerPositions.CAM));
        team.Players.Add(GeneratePlayer(PlayerPositions.LW));
        team.Players.Add(GeneratePlayer(PlayerPositions.RW));
        team.Players.Add(GeneratePlayer(PlayerPositions.CF));
        for (int i = 0; i < playerCount; i++)
        {
            team.Players.Add(GeneratePlayer(PlayerPositions.NONE));
        }

        team.CalcAvg();
        team.CurrentFormation.InitPositions();
        return team;
    }

    public static Player GeneratePlayer(PlayerPositions pos)
    {
        string[] firstNames = { "John", "David", "Michael", "James", "Robert", "William", "Carlos", "Juan", "Pedro",
        "Antonio", "Cole", "Jimmy", "Charlie", "Tom", "Declan", "Loyd", "Carl", "Rick", "Wayne", "Danny", "Timmy",
        "Floyd", "Marcus", "Mark", "Luke", "Mason", "Reece", "Steve", "Jamie", "Graham", "Matt", "Walt", "Ben", "Neal"};
        string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Davis", "Garcia", "Rodriguez",
        "Martinez", "Wilson","Baines","Archer","Baker","Barker","Bowman","Coates","Charlton","Davis","Cowley",
        "Dodds","Dyer","Fanning","Eastwood","Edwards","Fletcher","Fox","GoodWyn","Garner","Gold","Higgs","Hayes",
        "Jacobs","Jones","Keane","Lawson","MacDonald","Maddison","McCann","Miller","O'niel","Newman","Parker",
        "Newton","Payne","Radford","Ramsey","Reed","Stephens","Smith","Wally","Watkins","Wells","Wilkinson","Young","Yates"};

        string name = $"{firstNames[_random.Next(firstNames.Length)]} {lastNames[_random.Next(lastNames.Length)]}";
        
        int ageRand = _random.Next(20);
        int age;
        if (ageRand < 3){
            age = _random.Next(16, 22);
        }
        else if (ageRand < 16){
            age = _random.Next(20, 33);
        }
        else{
            age = _random.Next(30, 37);
        }
        
        
        Player player = new Player
        {
            Name = name,
            Age = age,
            Positions = new List<PlayerPositions>(),
            Attributes = new Dictionary<Attributes, int>()
        };

        PlayerPositions primaryPosition = PlayerPositions.NONE;
        if (pos == PlayerPositions.NONE)
        {

            do
            {
                primaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length - 10);
            }
            while (primaryPosition == PlayerPositions.NONE);
            player.Positions.Add(primaryPosition);
        }
        else
        {
            primaryPosition = pos;
            player.Positions.Add(primaryPosition);
        }

        PlayerPositions secondaryPosition = PlayerPositions.NONE;
        if (_random.Next(100) < 35 && primaryPosition != PlayerPositions.GK)
        {
            do
            {
                secondaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length - 10);
            } while (secondaryPosition == primaryPosition || secondaryPosition == PlayerPositions.GK || secondaryPosition == PlayerPositions.NONE);

            player.Positions.Add(secondaryPosition);
        }

        int rand = _random.Next(20);
        foreach (Attributes attribute in Enum.GetValues(typeof(Attributes)))
        {
            int baseValue;
            if (rand == 1)
            {
                baseValue = _random.Next(45, 95);
            }
            else if (rand < 5)
            {
                baseValue = _random.Next(35, 90);
            }
            else if (rand < 15)
            {
                baseValue = _random.Next(35, 80);
            }
            else
            {
                baseValue = _random.Next(25, 60);
            }



            if (IsAttributeRelevantToPosition(attribute, primaryPosition) || IsAttributeRelevantToPosition(attribute, secondaryPosition))
            {
                if (rand == 1)
                {
                    baseValue = _random.Next(80, 99);
                }
                else if (rand < 5)
                {
                    baseValue = _random.Next(70, 95);
                }
                else if (rand < 15)
                {
                    baseValue = _random.Next(60, 85);
                }
                else
                {
                    baseValue = _random.Next(30, 70);
                }
            }

            if (primaryPosition == PlayerPositions.GK && !IsAttributeRelevantToPosition(attribute, primaryPosition))
            {
                baseValue = _random.Next(30, 60);
            }
            if (primaryPosition == PlayerPositions.GK && attribute == Attributes.FlairSkills)
            {
                baseValue = _random.Next(20, 75);
            }
            player.Attributes[attribute] = Math.Min(baseValue, 99);
        }

        if (primaryPosition != PlayerPositions.GK)
        {
            player.Attributes[Attributes.Reflexes] = _random.Next(10, 50);
            player.Attributes[Attributes.Sweeping] = _random.Next(10, 50);
            player.Attributes[Attributes.Diving] = _random.Next(10, 50);
        }






        float ovr = 0;
        int cnt = 0;
        foreach (var att in player.Attributes)
        {
            if (IsAttributeRelevantToPosition(att.Key, primaryPosition))
            {
                ovr += att.Value;
                cnt++;
            }
        }
        player.Overall = (int)(ovr / cnt);

        return player;
    }

    private static bool IsAttributeRelevantToPosition(Attributes attribute, PlayerPositions position)
    {
        if (position == PlayerPositions.NONE) { return false; }

        if (attribute == Attributes.Speed
        || attribute == Attributes.Strength
        || attribute == Attributes.Stanima
        || attribute == Attributes.FlairSkills
        || attribute == Attributes.Composure
        || attribute == Attributes.Leadership
        || attribute == Attributes.Drive
        || attribute == Attributes.BallControl)
        {
            return true;
        }
        switch (position)
        {
            case PlayerPositions.GK:
                return attribute == Attributes.Reflexes || attribute == Attributes.Diving || attribute == Attributes.Sweeping || 
                attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision;

            case PlayerPositions.CB:
            case PlayerPositions.LCB:
            case PlayerPositions.RCB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.Strength || 
                attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LB:
            case PlayerPositions.RB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.ShortPassing || 
                attribute == Attributes.LongPassing || attribute == Attributes.Crossing;

            case PlayerPositions.CDM:
            case PlayerPositions.LDM:
            case PlayerPositions.RDM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting || 
                attribute == Attributes.Tackling || attribute == Attributes.Sliding || attribute == Attributes.Strength;
            case PlayerPositions.CM:
            case PlayerPositions.LCM:
            case PlayerPositions.RCM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting || 
                attribute == Attributes.BallControl || attribute == Attributes.Dribbling || attribute == Attributes.Vision;
            case PlayerPositions.CAM:
            case PlayerPositions.LAM:
            case PlayerPositions.RAM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision || 
                attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.LongShooting;
            case PlayerPositions.LM:
            case PlayerPositions.RM:
                return attribute == Attributes.Crossing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || 
                attribute == Attributes.BallControl || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LWB:
            case PlayerPositions.RWB:
                return attribute == Attributes.Tackling || attribute == Attributes.Crossing || attribute == Attributes.Vision || 
                attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LW:
            case PlayerPositions.RW:
                return attribute == Attributes.Crossing || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || 
                attribute == Attributes.Vision || attribute == Attributes.Finishing || attribute == Attributes.LongShooting || 
                attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.CF:
            case PlayerPositions.LF:
            case PlayerPositions.RF:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || 
                attribute == Attributes.BallControl;
            case PlayerPositions.F9:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || 
                attribute == Attributes.BallControl || attribute == Attributes.Vision || attribute == Attributes.ShortPassing;

            default:
                return false;
        }
    }

    public static List<Team> GenerateLeague(int teamCount)
    {
        List<Team> teams = new List<Team>();
        string[] NamePart1 = { "Black", "White", "Red", "North", "South", "West", "East", "Light", "River", "Stam", "Barn", "Brom", "Swind", "Flat", "Mount", "Clif", "Hawk", "Lion", "Bear", "Carl", "Brim", "Hig", "High", "Low", "Chels", "Grim", "Raven", "Green" };
        string[] NamePart2 = { "pool", "ton", "ham", "dale", "sea", "stone", "rock", "tree", "worth", "ing", "head", "ster", "ford", "sall", "gate", "ley", "mere", "field", "port", "ampton", "bury", "bridge", "borough", "age", "wood", "forest" };
        string[] NamePart3 = { "City", "County", "Town", "Athletic", "United", "Rovers", "FC", "Forest" };

        string teamName;
        for (int i = 0; i < teamCount; i++)
        {
            string Part1 = NamePart1[_random.Next(NamePart1.Length)];
            string Part2;
            string Part3;
            do
            {
                Part2 = NamePart2[_random.Next(NamePart2.Length)];
            } while (Part2.ToLower() == Part1.ToLower());

            do
            {
                Part3 = NamePart3[_random.Next(NamePart3.Length)];
            } while (Part3.ToLower() == Part1.ToLower() || Part3.ToLower() == Part2.ToLower());

            teamName = $"{Part1}{Part2} {Part3}";
            string teamNameShort = $"{Part1[0]}{Part2[0]}{Part3[0]}";
            teams.Add(GenerateTeam(teamName, teamNameShort));
        }

        return teams;
    }

    public static void UpdateLiveOverall(Player player, PlayerPositions position){


        List<double> values = new List<double>();

        foreach (var pos in player.Positions){
            switch (pos)
            {
                case PlayerPositions.GK:
                    if (position != PlayerPositions.GK){
                        values.Add(0.5);
                    }
                    else{
                        values.Add(1.0);
                    }
                    break;
                case PlayerPositions.CB:
                    if (position == PlayerPositions.CB || position == PlayerPositions.LCB
                    || position == PlayerPositions.RCB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.LB || position == PlayerPositions.RB
                    || position == PlayerPositions.CDM){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.LB:
                    if (position == PlayerPositions.LB || position == PlayerPositions.LWB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.RB || position == PlayerPositions.RWB
                    || position == PlayerPositions.LM || position == PlayerPositions.LCB){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.RB:
                    if (position == PlayerPositions.RB || position == PlayerPositions.RWB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.LB || position == PlayerPositions.LWB
                    || position == PlayerPositions.RM || position == PlayerPositions.RCB ){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.CDM:
                    if (position == PlayerPositions.CDM || position == PlayerPositions.LDM
                    || position == PlayerPositions.RDM || position == PlayerPositions.CM
                    || position == PlayerPositions.LCM || position == PlayerPositions.RCM){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.CAM || position == PlayerPositions.LAM
                    || position == PlayerPositions.RAM || position == PlayerPositions.CB
                    || position == PlayerPositions.LCB || position == PlayerPositions.RCB){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.CM:
                    if (position == PlayerPositions.CM || position == PlayerPositions.LCM
                    || position == PlayerPositions.RCM || position == PlayerPositions.CDM
                    || position == PlayerPositions.LDM || position == PlayerPositions.RDM
                    || position == PlayerPositions.CAM || position == PlayerPositions.LAM
                    || position == PlayerPositions.RAM){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.LM || position == PlayerPositions.RM){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.CAM:
                    if (position == PlayerPositions.CAM || position == PlayerPositions.LAM
                    || position == PlayerPositions.RAM || position == PlayerPositions.CM
                    || position == PlayerPositions.LCM || position == PlayerPositions.RCM){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.CDM || position == PlayerPositions.LDM
                    || position == PlayerPositions.RDM || position == PlayerPositions.F9){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.LM:
                    if (position == PlayerPositions.LM || position == PlayerPositions.LW
                    || position == PlayerPositions.LWB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.RM || position == PlayerPositions.RW
                    || position == PlayerPositions.RWB || position == PlayerPositions.LB
                    || position == PlayerPositions.LCM || position == PlayerPositions.LAM){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.RM:
                    if (position == PlayerPositions.RM || position == PlayerPositions.RW
                    || position == PlayerPositions.RWB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.LM || position == PlayerPositions.LW
                    || position == PlayerPositions.LWB || position == PlayerPositions.RB
                    || position == PlayerPositions.RCM || position == PlayerPositions.RAM){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.F9:
                    if (position == PlayerPositions.F9 || position == PlayerPositions.CF
                    || position == PlayerPositions.LF || position == PlayerPositions.RF){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.CAM || position == PlayerPositions.LAM
                    || position == PlayerPositions.RAM){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.LW:
                    if (position == PlayerPositions.LW || position == PlayerPositions.LM){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.LF || position == PlayerPositions.LAM
                    || position == PlayerPositions.RW || position == PlayerPositions.LWB){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.RW:
                    if (position == PlayerPositions.RW || position == PlayerPositions.RM){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.RF || position == PlayerPositions.RAM
                    || position == PlayerPositions.LW || position == PlayerPositions.RWB){
                        values.Add(0.8);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                case PlayerPositions.CF:
                    if (position == PlayerPositions.CF || position == PlayerPositions.LF
                    || position == PlayerPositions.RF || position == PlayerPositions.F9){
                        values.Add(1.0);
                    }
                    else{
                        values.Add(0.5);
                    }
                    break;
                
            }

        }

        player.LiveOverall = (int)(player.Overall * values.Max());

    }
}