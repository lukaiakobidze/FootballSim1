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
        team.Players.Add(GeneratePlayer(PlayerPositions.LB));
        team.Players.Add(GeneratePlayer(PlayerPositions.RB));
        team.Players.Add(GeneratePlayer(PlayerPositions.CM));
        team.Players.Add(GeneratePlayer(PlayerPositions.CDM));
        team.Players.Add(GeneratePlayer(PlayerPositions.CAM));
        team.Players.Add(GeneratePlayer(PlayerPositions.LW));
        team.Players.Add(GeneratePlayer(PlayerPositions.RW));
        team.Players.Add(GeneratePlayer(PlayerPositions.ST));
        for (int i = 0; i < playerCount; i++)
        {
            team.Players.Add(GeneratePlayer(PlayerPositions.NONE));
        }

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
                primaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
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
                secondaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
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
                return attribute == Attributes.Reflexes || attribute == Attributes.Diving || attribute == Attributes.Sweeping || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision;

            case PlayerPositions.CB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.Strength || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LB:
            case PlayerPositions.RB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Crossing;

            case PlayerPositions.CDM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting || attribute == Attributes.Tackling || attribute == Attributes.Sliding || attribute == Attributes.Strength;
            case PlayerPositions.CM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting || attribute == Attributes.BallControl || attribute == Attributes.Dribbling || attribute == Attributes.Vision;

            case PlayerPositions.CAM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.LongShooting;
            case PlayerPositions.LM:
            case PlayerPositions.RM:
                return attribute == Attributes.Crossing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LWB:
            case PlayerPositions.RWB:
                return attribute == Attributes.Tackling || attribute == Attributes.Crossing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;

            case PlayerPositions.LW:
            case PlayerPositions.RW:
                return attribute == Attributes.Crossing || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.Vision || attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;

            case PlayerPositions.ST:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || attribute == Attributes.BallControl;
            case PlayerPositions.CF:
            case PlayerPositions.LF:
            case PlayerPositions.RF:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.Vision || attribute == Attributes.ShortPassing;

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
}