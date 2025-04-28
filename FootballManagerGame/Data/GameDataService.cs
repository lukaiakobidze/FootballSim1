using System;
using System.Collections.Generic;


namespace FootballManagerGame.Data;

using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class GameDataService
{
    private readonly string _saveDirectory;
    private const string GameStateFileName = "gamestate.json";

    public GameDataService(string saveDirectory = "Saves")
    {
        _saveDirectory = saveDirectory;
        Directory.CreateDirectory(_saveDirectory);
    }

    public void SaveGame(GameState gameState, string saveName = "default")
    {
        Console.WriteLine($"{gameState.PlayerLeague.teams.Count}");
        string saveFolder = Path.Combine(_saveDirectory, saveName);
        Directory.CreateDirectory(saveFolder);

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        string gameStateJson = JsonConvert.SerializeObject(gameState, settings);
        File.WriteAllText(Path.Combine(saveFolder, GameStateFileName), gameStateJson);
    }

    public GameState LoadGame(string saveName = "default")
    {
        string saveFolder = Path.Combine(_saveDirectory, saveName);
        string gameStateFile = Path.Combine(saveFolder, GameStateFileName);

        if (!File.Exists(gameStateFile))
            return null;

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        string gameStateJson = File.ReadAllText(gameStateFile);
        GameState gameState = JsonConvert.DeserializeObject<GameState>(gameStateJson, settings);
        
        return gameState;
    }

    public List<string> GetSaveGames()
    {
        return Directory.GetDirectories(_saveDirectory)
            .Select(Path.GetFileName)
            .ToList();
    }

    public bool DeleteSaveGame(string saveName)
    {
        try
        {
            string saveFolder = Path.Combine(_saveDirectory, saveName);


            if (Directory.Exists(saveFolder))
            {

                foreach (string file in Directory.GetFiles(saveFolder))
                {
                    File.Delete(file);
                }


                Directory.Delete(saveFolder);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error deleting save game: {ex.Message}");
            return false;
        }
    }
}