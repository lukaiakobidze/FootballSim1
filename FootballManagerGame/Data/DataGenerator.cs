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
    public static Random Random = new Random();

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

    public static void SimFixture(Fixture fixture)
    {
        if (fixture.Team1.CurrentFormation.IsComplete() && fixture.Team2.CurrentFormation.IsComplete())
        {
            if (!fixture.Completed)
            {
                fixture.Team1.CalcAvg();
                fixture.Team2.CalcAvg();

                fixture.FirstHalfTime = _random.Next(45, 52);
                fixture.SecondHalfTime = _random.Next(45, 52);
                int totalTime = fixture.FirstHalfTime + fixture.SecondHalfTime;


                int team1ChanceCap = (int)(fixture.Team1.AvgOvr * 0.25);
                int team2ChanceCap = (int)(fixture.Team2.AvgOvr * 0.25);

                fixture.team1ChancesCreated = _random.Next(1, team1ChanceCap + 1);
                fixture.team2ChancesCreated = _random.Next(1, team2ChanceCap + 1);


                fixture.team1ShotAttempts = Enumerable.Range(0, fixture.team1ChancesCreated)
                    .Count(_ => _random.NextDouble() < 0.85);
                fixture.team2ShotAttempts = Enumerable.Range(0, fixture.team2ChancesCreated)
                    .Count(_ => _random.NextDouble() < 0.85);


                fixture.team1ShotOnTarget = Enumerable.Range(0, fixture.team1ShotAttempts)
                    .Count(_ => _random.NextDouble() < 0.4);
                fixture.team2ShotOnTarget = Enumerable.Range(0, fixture.team2ShotAttempts)
                    .Count(_ => _random.NextDouble() < 0.4);

                int ovrDiff = fixture.Team1.AvgOvr - fixture.Team2.AvgOvr;
                double baseChance = 0.2;
                double diffImpact = 0.02;

                double team1GoalChance = baseChance + (ovrDiff * diffImpact);
                team1GoalChance = Math.Clamp(team1GoalChance, 0.1, 0.7);
                double team2GoalChance = baseChance - (ovrDiff * diffImpact);
                team2GoalChance = Math.Clamp(team2GoalChance, 0.1, 0.7);

                int team1Goals = Enumerable.Range(0, fixture.team1ShotOnTarget)
                    .Count(_ => _random.NextDouble() < team1GoalChance);
                int team2Goals = Enumerable.Range(0, fixture.team2ShotOnTarget)
                    .Count(_ => _random.NextDouble() < team2GoalChance);

                List<Goal> goals = new List<Goal>();

                for (int i = 0; i < team1Goals; i++)
                {
                    int time = _random.Next(fixture.FirstHalfTime + fixture.SecondHalfTime);
                    Player scorer = null;
                    Player assistant = null;
                    var validPos = fixture.Team1.CurrentFormation.Positions
                        .Where(pos => fixture.Team1.CurrentFormation.Players.ContainsKey(pos))
                        .ToList();
                    do
                    {
                        scorer = fixture.Team1.CurrentFormation.Players[validPos[_random.Next(validPos.Count)]];
                        assistant = fixture.Team1.CurrentFormation.Players[validPos[_random.Next(validPos.Count)]];
                    } while (scorer == null);

                    if (scorer == assistant)
                    {
                        assistant = null;
                    }
                    goals.Add(new Goal { TimeScored = time, PlayerScored = scorer, PlayerAssisted = assistant });
                }

                for (int i = 0; i < team2Goals; i++)
                {
                    int time = _random.Next(fixture.FirstHalfTime + fixture.SecondHalfTime);
                    Player scorer = null;
                    Player assistant = null;
                    var validPos = fixture.Team2.CurrentFormation.Positions
                        .Where(pos => fixture.Team2.CurrentFormation.Players.ContainsKey(pos))
                        .ToList();
                    do
                    {
                        scorer = fixture.Team2.CurrentFormation.Players[validPos[_random.Next(validPos.Count)]];
                        assistant = fixture.Team2.CurrentFormation.Players[validPos[_random.Next(validPos.Count)]];
                    } while (scorer == null);

                    if (scorer == assistant)
                    {
                        assistant = null;
                    }
                    goals.Add(new Goal { TimeScored = time, PlayerScored = scorer, PlayerAssisted = assistant });
                }
                List<Goal> goalsSorted = goals.OrderBy(g => g.TimeScored).ToList();
                fixture.SetResult(team1Goals, team2Goals, goalsSorted);
                fixture.AddPoints();
            }
            else
            {
                throw new Exception("Fixture already simulated");
            }
        }
        else
        {
            throw new Exception($"Team formations arent complete!{fixture.Team1.Name} {fixture.Team2.Name}");
        }
    }

    public static void SimMinute(Fixture fixture, int minute, bool isLast)
    {
        if (fixture.Team1.CurrentFormation.IsComplete() && fixture.Team2.CurrentFormation.IsComplete())
        {
            fixture.Team1.CalcAvg();
            fixture.Team2.CalcAvg();

            int ovrDiff = fixture.Team1.AvgOvr - fixture.Team2.AvgOvr;

            if (_random.Next(10) < 4)
            {
                int rand = _random.Next(100);
                if (rand <= 50 + (ovrDiff * 2))
                {
                    fixture.team1ChancesCreated += 1;
                    if (_random.Next(10) < 7)
                    {
                        fixture.team1ShotAttempts += 1;
                        if (_random.Next(10) < 4)
                        {
                            fixture.team1ShotOnTarget += 1;
                            if (_random.Next(10) < 4)
                            {
                                Player scorer = PickGoalScorer(fixture.Team1.CurrentFormation.Players);
                                Player assistant = PickGoalAssistant(fixture.Team1.CurrentFormation.Players, scorer);

                                Goal goal = new Goal()
                                {
                                    TimeScored = minute,
                                    PlayerScored = scorer,
                                    PlayerAssisted = assistant,
                                    IsPenalty = false
                                };

                                fixture.AddGoal(goal);
                            }
                        }
                    }
                }
                else
                {
                    fixture.team2ChancesCreated += 1;
                    if (_random.Next(10) < 7)
                    {
                        fixture.team2ShotAttempts += 1;
                        if (_random.Next(10) < 4)
                        {
                            fixture.team2ShotOnTarget += 1;
                            if (_random.Next(10) < 4)
                            {
                                Player scorer = PickGoalScorer(fixture.Team2.CurrentFormation.Players);
                                Player assistant = PickGoalAssistant(fixture.Team2.CurrentFormation.Players, scorer);

                                Goal goal = new Goal()
                                {
                                    TimeScored = minute,
                                    PlayerScored = scorer,
                                    PlayerAssisted = assistant,
                                    IsPenalty = false
                                };

                                fixture.AddGoal(goal);
                            }
                        }
                    }
                }
            }

            if (isLast)
            {
                fixture.Completed = true;
                fixture.AddPoints();
            }
        }
        else
        {
            throw new Exception("Team formations arent complete!");
        }
    }

    public static Player PickGoalScorer(Dictionary<PlayerPositions, Player> players) {

        Dictionary<PlayerPositions, double> positionMultipliers = new Dictionary<PlayerPositions, double>
        {
            {PlayerPositions.GK, 0.1},
            {PlayerPositions.CB, 0.4},
            {PlayerPositions.LB, 0.5},
            {PlayerPositions.LWB, 0.8},
            {PlayerPositions.RB, 0.5},
            {PlayerPositions.RWB, 0.8},
            {PlayerPositions.CDM, 0.4},
            {PlayerPositions.CM, 1.0},
            {PlayerPositions.CAM, 1.9},
            {PlayerPositions.LM, 1.4},
            {PlayerPositions.LW, 2.0},
            {PlayerPositions.RM, 1.4},
            {PlayerPositions.RW, 2.0},
            {PlayerPositions.F9, 2.0},
            {PlayerPositions.CF, 2.6},
            {PlayerPositions.LCB, 0.4},
            {PlayerPositions.RCB, 0.4},
            {PlayerPositions.LCM, 1.0},
            {PlayerPositions.RCM, 1.0},
            {PlayerPositions.LDM, 0.4},
            {PlayerPositions.RDM, 0.4},
            {PlayerPositions.LAM, 1.9},
            {PlayerPositions.RAM, 1.9},
            {PlayerPositions.LF, 2.6},
            {PlayerPositions.RF, 2.6},
        };

        var playerList = players.Values.ToList();

        List<double> weights = players.Select(kvp =>
        {
            PlayerPositions currentPosition = kvp.Key;
            Player player = kvp.Value;

            double mult = positionMultipliers.ContainsKey(currentPosition)
                        ? positionMultipliers[currentPosition]
                        : 0.5;

            return player.Overall * mult;

        }).ToList();
        
        double totalWeight = weights.Sum();
        double randValue = _random.NextDouble() * totalWeight;

        double cumulative = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            cumulative += weights[i];
            if (randValue <= cumulative)
                return playerList[i];
        }

        return playerList.Last();
    }


    public static Player PickGoalAssistant(Dictionary<PlayerPositions, Player> players, Player scorer) {

        Dictionary<PlayerPositions, double> positionMultipliers = new Dictionary<PlayerPositions, double>
        {
            {PlayerPositions.GK, 0.2},
            {PlayerPositions.CB, 0.4},
            {PlayerPositions.LB, 0.8},
            {PlayerPositions.LWB, 1.2},
            {PlayerPositions.RB, 0.8},
            {PlayerPositions.RWB, 1.2},
            {PlayerPositions.CDM, 1.0},
            {PlayerPositions.CM, 1.5},
            {PlayerPositions.CAM, 2.2},
            {PlayerPositions.LM, 2.0},
            {PlayerPositions.LW, 2.8},
            {PlayerPositions.RM, 2.0},
            {PlayerPositions.RW, 2.8},
            {PlayerPositions.F9, 2.8},
            {PlayerPositions.CF, 2.6},
            {PlayerPositions.LCB, 0.4},
            {PlayerPositions.RCB, 0.4},
            {PlayerPositions.LCM, 1.5},
            {PlayerPositions.RCM, 1.5},
            {PlayerPositions.LDM, 1.0},
            {PlayerPositions.RDM, 1.0},
            {PlayerPositions.LAM, 2.2},
            {PlayerPositions.RAM, 2.2},
            {PlayerPositions.LF, 2.6},
            {PlayerPositions.RF, 2.6},
        };
        
        var playerList = players.Values.ToList();
        

        List<double> weights = players.Select(kvp =>
        {
            PlayerPositions currentPosition = kvp.Key;
            Player player = kvp.Value;



            double mult = positionMultipliers.ContainsKey(currentPosition)
                        ? positionMultipliers[currentPosition]
                        : 0.5;

            if (player == scorer)
            {
                return 0;
            }

            return player.Overall * mult;

        }).ToList();
        
        double totalWeight = weights.Sum();
        double randValue = _random.NextDouble() * totalWeight;

        double cumulative = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            cumulative += weights[i];
            if (randValue <= cumulative)
                return playerList[i];
        }

        return playerList.Last();
    }


    public static Team GenerateTeam(string name, string nameShort)
    {
        Team team = new Team
        {
            Name = name,
            NameShort = nameShort,
            Players = new List<Player>(),
            CurrentFormation = new FourFourTwoFormation()
        };


        int playerCount = _random.Next(7, 12);
        team.Players.Add(GeneratePlayer(PlayerPositions.GK, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CB, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CB, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.LB, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.RB, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CM, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CDM, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CAM, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.LW, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.RW, team));
        team.Players.Add(GeneratePlayer(PlayerPositions.CF, team));
        for (int i = 0; i < playerCount; i++)
        {
            team.Players.Add(GeneratePlayer(PlayerPositions.NONE, team));
        }

        team.CurrentFormation.InitPositions();

        foreach (var pos in team.CurrentFormation.Positions)
        {
            Player pl = team.Players
                .Where(p => p.Positions != null && p.Positions.Count > 0 && !team.CurrentFormation.Players.ContainsValue(p))
                .Select(p => new
                {
                    Player = p,
                    BestFitScore = p.Positions.Max(plPos => PositionCompatibility(plPos, pos))
                })
                .Where(x => x.BestFitScore >= 0)
                .OrderByDescending(x => x.BestFitScore)
                .ThenByDescending(x => x.Player.Overall)
                .Select(x => x.Player)
                .FirstOrDefault();

            if (pl != null)
            {
                team.CurrentFormation.Players[pos] = pl;
                UpdateLiveOverall(pl, pos);
            }
            else
            {
                team.CurrentFormation.Players[pos] = team.Players.Where(p => !team.CurrentFormation.Players.ContainsValue(p)).FirstOrDefault();
            }

        }

        return team;
    }

    public static Player GeneratePlayer(PlayerPositions pos, Team team)
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
                primaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
            }
            while (
                primaryPosition == PlayerPositions.NONE || primaryPosition == PlayerPositions.LCB || primaryPosition == PlayerPositions.RCB
                || primaryPosition == PlayerPositions.LDM || primaryPosition == PlayerPositions.RDM || primaryPosition == PlayerPositions.LCM
                || primaryPosition == PlayerPositions.RCM || primaryPosition == PlayerPositions.LAM || primaryPosition == PlayerPositions.RAM
                || primaryPosition == PlayerPositions.LF || primaryPosition == PlayerPositions.RF 
            );

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
                secondaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
            }
            while (
                secondaryPosition == primaryPosition || secondaryPosition == PlayerPositions.GK || secondaryPosition == PlayerPositions.NONE
                || secondaryPosition == PlayerPositions.LCB || secondaryPosition == PlayerPositions.RCB
                || secondaryPosition == PlayerPositions.LDM || secondaryPosition == PlayerPositions.RDM || secondaryPosition == PlayerPositions.LCM
                || secondaryPosition == PlayerPositions.RCM || secondaryPosition == PlayerPositions.LAM || secondaryPosition == PlayerPositions.RAM
                || secondaryPosition == PlayerPositions.LF || secondaryPosition == PlayerPositions.RF
            );

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

        player.Team = team;

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
            teamNameShort = teamNameShort.ToUpper();
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
                case PlayerPositions.RWB:
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
                case PlayerPositions.LWB:
                    if (position == PlayerPositions.LWB || position == PlayerPositions.LB){
                        values.Add(1.0);
                    }
                    else if (position == PlayerPositions.RB || position == PlayerPositions.RWB
                    || position == PlayerPositions.LM || position == PlayerPositions.LW ){
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
        if (values.Count == 0)
        {
            player.LiveOverall = player.Overall / 2; // or 0, or some fallback
        }
        else
        {
            player.LiveOverall = (int)(player.Overall * values.Max());
        }
        

    }

    public static int PositionCompatibility(PlayerPositions playerPos, PlayerPositions targetPos){

        switch (playerPos)
        {
            case PlayerPositions.GK:
                if (targetPos != PlayerPositions.GK){
                    return -1;
                }
                else{
                    return 1;
                }
                
            case PlayerPositions.CB:
                if (targetPos == PlayerPositions.CB || targetPos == PlayerPositions.LCB
                || targetPos == PlayerPositions.RCB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LB || targetPos == PlayerPositions.RB
                || targetPos == PlayerPositions.CDM){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.LB:
                if (targetPos == PlayerPositions.LB || targetPos == PlayerPositions.LWB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.RB || targetPos == PlayerPositions.RWB
                || targetPos == PlayerPositions.LM || targetPos == PlayerPositions.LCB){
                    return 0;
                }
                else{
                    return -1;
                }
            case PlayerPositions.LWB:
                if (targetPos == PlayerPositions.LB || targetPos == PlayerPositions.LWB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.RB || targetPos == PlayerPositions.RWB
                || targetPos == PlayerPositions.LM || targetPos == PlayerPositions.LW){
                    return 0;
                }
                else{
                    return -1;
                }
            case PlayerPositions.RWB:
                if (targetPos == PlayerPositions.RWB || targetPos == PlayerPositions.RB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LB || targetPos == PlayerPositions.LWB
                || targetPos == PlayerPositions.RW || targetPos == PlayerPositions.RM){
                    return 0;
                }
                else{
                    return -1;
                }
            case PlayerPositions.RB:
                if (targetPos == PlayerPositions.RB || targetPos == PlayerPositions.RWB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LB || targetPos == PlayerPositions.LWB
                || targetPos == PlayerPositions.RM || targetPos == PlayerPositions.RCB ){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.CDM:
                if (targetPos == PlayerPositions.CDM || targetPos == PlayerPositions.LDM
                || targetPos == PlayerPositions.RDM || targetPos == PlayerPositions.CM
                || targetPos == PlayerPositions.LCM || targetPos == PlayerPositions.RCM){
                    return 1;
                }
                else if (targetPos == PlayerPositions.CAM || targetPos == PlayerPositions.LAM
                || targetPos == PlayerPositions.RAM || targetPos == PlayerPositions.CB
                || targetPos == PlayerPositions.LCB || targetPos == PlayerPositions.RCB){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.CM:
                if (targetPos == PlayerPositions.CM || targetPos == PlayerPositions.LCM
                || targetPos == PlayerPositions.RCM || targetPos == PlayerPositions.CDM
                || targetPos == PlayerPositions.LDM || targetPos == PlayerPositions.RDM
                || targetPos == PlayerPositions.CAM || targetPos == PlayerPositions.LAM
                || targetPos == PlayerPositions.RAM){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LM || targetPos == PlayerPositions.RM){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.CAM:
                if (targetPos == PlayerPositions.CAM || targetPos == PlayerPositions.LAM
                || targetPos == PlayerPositions.RAM || targetPos == PlayerPositions.CM
                || targetPos == PlayerPositions.LCM || targetPos == PlayerPositions.RCM){
                    return 1;
                }
                else if (targetPos == PlayerPositions.CDM || targetPos == PlayerPositions.LDM
                || targetPos == PlayerPositions.RDM || targetPos == PlayerPositions.F9){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.LM:
                if (targetPos == PlayerPositions.LM || targetPos == PlayerPositions.LW
                || targetPos == PlayerPositions.LWB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.RM || targetPos == PlayerPositions.RW
                || targetPos == PlayerPositions.RWB || targetPos == PlayerPositions.LB
                || targetPos == PlayerPositions.LCM || targetPos == PlayerPositions.LAM){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.RM:
                if (targetPos == PlayerPositions.RM || targetPos == PlayerPositions.RW
                || targetPos == PlayerPositions.RWB){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LM || targetPos == PlayerPositions.LW
                || targetPos == PlayerPositions.LWB || targetPos == PlayerPositions.RB
                || targetPos == PlayerPositions.RCM || targetPos == PlayerPositions.RAM){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.F9:
                if (targetPos == PlayerPositions.F9 || targetPos == PlayerPositions.CF
                || targetPos == PlayerPositions.LF || targetPos == PlayerPositions.RF){
                    return 1;
                }
                else if (targetPos == PlayerPositions.CAM || targetPos == PlayerPositions.LAM
                || targetPos == PlayerPositions.RAM){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.LW:
                if (targetPos == PlayerPositions.LW || targetPos == PlayerPositions.LM){
                    return 1;
                }
                else if (targetPos == PlayerPositions.LF || targetPos == PlayerPositions.LAM
                || targetPos == PlayerPositions.RW || targetPos == PlayerPositions.LWB){
                   return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.RW:
                if (targetPos == PlayerPositions.RW || targetPos == PlayerPositions.RM){
                    return 1;
                }
                else if (targetPos == PlayerPositions.RF || targetPos == PlayerPositions.RAM
                || targetPos == PlayerPositions.LW || targetPos == PlayerPositions.RWB){
                    return 0;
                }
                else{
                    return -1;
                }
                
            case PlayerPositions.CF:
                if (targetPos == PlayerPositions.CF || targetPos == PlayerPositions.LF
                || targetPos == PlayerPositions.RF || targetPos == PlayerPositions.F9){
                    return 1;
                }
                else{
                    return -1;
                }

            default:
                return -1;
        }

    }
}