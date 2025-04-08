using System;
using System.Collections.Generic;
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

        
        int playerCount = _random.Next(20, 26);
        for (int i = 0; i < playerCount; i++)
        {
            team.Players.Add(GeneratePlayer());
        }

        return team;
    }

    public static Player GeneratePlayer()
    {
        string[] firstNames = { "John", "David", "Michael", "James", "Robert", "William", "Carlos", "Juan", "Pedro", "Antonio" };
        string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Davis", "Garcia", "Rodriguez", "Martinez", "Wilson" };

        string name = $"{firstNames[_random.Next(firstNames.Length)]} {lastNames[_random.Next(lastNames.Length)]}";
        int age = _random.Next(17, 36);
        PlayerPositions position = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);

        Player player = new Player
        {
            Name = name,
            Age = age,
            Positions = new List<PlayerPositions>(),
            Attributes = new Dictionary<Attributes, int>()
        };

        
        PlayerPositions primaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
        player.Positions.Add(primaryPosition);

        
        if (_random.Next(100) < 30)
        {
            PlayerPositions secondaryPosition;
            do
            {
                secondaryPosition = (PlayerPositions)_random.Next(Enum.GetValues(typeof(PlayerPositions)).Length);
            } while (secondaryPosition == primaryPosition);

            player.Positions.Add(secondaryPosition);
        }


        foreach (Attributes attribute in Enum.GetValues(typeof(Attributes)))
        {

            int baseValue = _random.Next(20, 44);


            if (IsAttributeRelevantToPosition(attribute, primaryPosition))
            {
                baseValue += _random.Next(25, 55);
            }

            
            player.Attributes[attribute] = Math.Min(baseValue, 99);
        }

        return player;
    }

    private static bool IsAttributeRelevantToPosition(Attributes attribute, PlayerPositions position)
    {
        
        if (attribute == Attributes.Speed
        || attribute == Attributes.Strength
        || attribute == Attributes.Stanima
        || attribute == Attributes.FlairSkills
        || attribute == Attributes.Composure
        || attribute == Attributes.Leadership
        || attribute == Attributes.Drive)
        {
            return true;
        }
        switch (position)
        {
            case PlayerPositions.GK:
                return attribute == Attributes.Reflexes || attribute == Attributes.Diving || attribute == Attributes.Sweeping || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;

            case PlayerPositions.CB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.Strength || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;
            case PlayerPositions.LB:
            case PlayerPositions.RB:
                return attribute == Attributes.Sliding || attribute == Attributes.Tackling || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Crossing;

            case PlayerPositions.CDM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting || attribute == Attributes.Tackling || attribute == Attributes.Sliding || attribute == Attributes.Strength;
            case PlayerPositions.CM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.LongShooting;

            case PlayerPositions.CAM:
                return attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.LongShooting;
            case PlayerPositions.LM:
            case PlayerPositions.RM:
                return attribute == Attributes.Crossing || attribute == Attributes.Vision || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing || attribute == Attributes.Vision;

            case PlayerPositions.LW:
            case PlayerPositions.RW:
                return attribute == Attributes.Crossing || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.Vision || attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.ShortPassing || attribute == Attributes.LongPassing;

            case PlayerPositions.ST:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || attribute == Attributes.BallControl;
            case PlayerPositions.CF:
                return attribute == Attributes.Finishing || attribute == Attributes.LongShooting || attribute == Attributes.Dribbling || attribute == Attributes.BallControl || attribute == Attributes.Vision || attribute == Attributes.ShortPassing;

            default:
                return false;
        }
    }

    public static List<Team> GenerateLeague(string leagueName, int teamCount)
    {
        List<Team> teams = new List<Team>();
        string[] NamePart1 = { "Black", "White", "Red", "North", "South", "West", "East", "Light", "River", "Stam", "Barn", "Brom", "Swind", "Flat", "Mount", "Clif", "Hawk", "Lion", "Bear" };
        string[] NamePart2 = { "pool", "ton", "ham", "dale", "sea", "stone", "rock", "tree", "worth", "ing", "head", "ster", "ford", "sall", "gate", "ley", "mere", "field", "port", "ampton", "bury", "bridge", "borough", "age", "wood", "forest" };
        string[] NamePart3 = { "City", "County", "Town", "Athletic", "United", "Rovers", "FC", "Forest" };


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

            string teamName = $"{Part1}{Part2} {Part3}";
            string teamNameShort = $"{Part1[0]}{Part2[0]}{Part3[0]}";
            teams.Add(GenerateTeam(teamName, teamNameShort));
        }

        return teams;
    }
}