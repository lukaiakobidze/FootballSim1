using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FootballManagerGame.Input;
using FootballManagerGame.Models;
using System.Linq;
using System.Collections.Generic;
using FootballManagerGame.Helpers;
using FootballManagerGame.Data;
using FootballManagerGame.Enums;
using System.Security.Cryptography.X509Certificates;
namespace FootballManagerGame.Views;


public class TeamStrategyScreen : Screen
{
    private GameState _gameState;
    private SpriteFont _font;
    private GraphicsDeviceManager _graphics;
    private ShapeDrawer _shapes;
    private List<Texture2D> _textures;
    private int _selectionIndex = 0;
    private int _selectedPlayerIndex = 0;
    private int _selectedPositionIndex = 0;
    private int _selectedFormationIndex = 0;
    private bool _showPlayers = false;
    private bool _showPositions = false;
    private bool _showFormations = false;
    private List<string> _strings;
    private List<string> _formations;
    private List<Player> _orderedList;
    private PlayerPositions _pos;
    private Player _switcher;
    private int _switcherIndex;


    public TeamStrategyScreen(GameState gameState, SpriteFont font, GraphicsDeviceManager graphics, ShapeDrawer shapes, List<Texture2D> textures)
    {
        _graphics = graphics;
        _gameState = gameState;
        _font = font;
        _shapes = shapes;
        _textures = textures;
        _strings = new List<string>() { "Change formation", "Change players" };
        _formations = new List<string>() { "4-4-2", "4-4-1-1", "4-3-3", "4-3-3 Attacking", "4-3-3 Defensive", "4-2-1-2-1", "4-1-2-1-2", "4-5-1", "5-3-2", "5-2-1-2", "5-4-1",
            "3-4-3", "3-4-1-2"};
        _pos = PlayerPositions.NONE;

        _orderedList = _gameState.TeamSelected.Players
            .OrderByDescending(p => p.Overall)
            .ThenBy(p => p.Positions.First())
            .ToList();

        _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
            || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));
    }

    public override void Update(GameTime gameTime)
    {
        _gameState.TeamSelected.CalcAvg();

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        if (_gameState.TeamSelected != null)
        {
            spriteBatch.DrawString(_font, _gameState.TeamSelected?.Name ?? "No Team Selected", new Vector2(100, 50), Color.White);

            if (_showPlayers)
            {
                if (_pos != PlayerPositions.NONE)
                {
                    _orderedList = _gameState.TeamSelected.Players
                    .OrderByDescending(p => p.Positions.Max(pos => DataGenerator.PositionCompatibility(pos, _pos)))
                    .ThenBy(p => p.Positions.Min(pos => Math.Abs((int)pos - (int)_pos)))
                    .ThenByDescending(p => p.Overall)
                    .ToList();
                    
                }
                else
                {
                    _orderedList = _gameState.TeamSelected.Players
                        .OrderByDescending(p => p.Overall)
                        .ThenBy(p => p.Positions.First())
                        .ToList();
                }
                _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
                || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));

                int y = 130;
                for (int i = 0; i < _orderedList.Count; i++)
                {
                    Color color = (i == _selectedPlayerIndex) ? Color.Yellow : Color.White;
                    Color colorOVR = Color.White;
                    string positions = string.Join("/", _orderedList[i].Positions);
                    spriteBatch.DrawString(_font, $"{_orderedList[i].Name} - {positions} - Age: {_orderedList[i].Age}", new Vector2(100, y), color);

                    if (_orderedList[i].Overall < 40) { colorOVR = Color.IndianRed; }
                    else if (_orderedList[i].Overall < 60) { colorOVR = Color.Orange; }
                    else if (_orderedList[i].Overall < 70) { colorOVR = Color.Yellow; }
                    else if (_orderedList[i].Overall < 80) { colorOVR = Color.LightGreen; }
                    else if (_orderedList[i].Overall < 90) { colorOVR = Color.SpringGreen; }
                    else { colorOVR = Color.Cyan; }
                    spriteBatch.DrawString(_font, $"{_orderedList[i].Overall}", new Vector2(500, y), colorOVR);
                    y += 30;
                }
            }
            else if (_showPositions)
            {

                int y = 130;
                for (int i = 0; i < 18; i++)
                {
                    Color color = (i == _selectedPositionIndex) ? Color.Yellow : Color.White;
                    Color colorOVR = Color.White;
                    if (i < 11)
                    {
                        spriteBatch.DrawString(_font, $"{_gameState.TeamSelected.CurrentFormation.Positions[i]}", new Vector2(100, y), color);

                        if (_gameState.TeamSelected.CurrentFormation.Players.ContainsKey(_gameState.TeamSelected.CurrentFormation.Positions[i]))
                        {
                            spriteBatch.DrawString(_font, $"{_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].Name}",
                                new Vector2(180, y), color);

                            spriteBatch.DrawString(_font, $"{string.Join("/", _gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].Positions)}",
                                new Vector2(450, y), color);

                            if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall < 40) { colorOVR = Color.IndianRed; }
                            else if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall < 60) { colorOVR = Color.Orange; }
                            else if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall < 70) { colorOVR = Color.Yellow; }
                            else if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall < 80) { colorOVR = Color.LightGreen; }
                            else if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall < 90) { colorOVR = Color.SpringGreen; }
                            else { colorOVR = Color.Cyan; }
                            spriteBatch.DrawString(_font, $"{_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]].LiveOverall}",
                                new Vector2(600, y), colorOVR);

                            if (_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]] == _switcher)
                            {
                                spriteBatch.DrawString(_font, "Switch", new Vector2(700, y), color);
                            }
                        }
                    }
                    else
                    {
                        if(i == 11) { y += 60; }
                        spriteBatch.DrawString(_font, "Sub", new Vector2(100, y), color);

                        if (_gameState.TeamSelected.CurrentFormation.Bench.ContainsKey(i - 11))
                        {
                            spriteBatch.DrawString(_font, $"{_gameState.TeamSelected.CurrentFormation.Bench[i - 11].Name}",
                                new Vector2(180, y), color);
                            
                            spriteBatch.DrawString(_font, $"{string.Join("/", _gameState.TeamSelected.CurrentFormation.Bench[i - 11].Positions)}",
                                new Vector2(450, y), color);

                            if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall < 40) { colorOVR = Color.IndianRed; }
                            else if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall < 60) { colorOVR = Color.Orange; }
                            else if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall < 70) { colorOVR = Color.Yellow; }
                            else if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall < 80) { colorOVR = Color.LightGreen; }
                            else if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall < 90) { colorOVR = Color.SpringGreen; }
                            else { colorOVR = Color.Cyan; }
                            spriteBatch.DrawString(_font, $"{_gameState.TeamSelected.CurrentFormation.Bench[i - 11].LiveOverall}",
                                new Vector2(600, y), colorOVR);

                            if (_gameState.TeamSelected.CurrentFormation.Bench[i - 11] == _switcher)
                            {
                                spriteBatch.DrawString(_font, "Switch", new Vector2(700, y), color);
                            }
                        }
                    }
                    
                    y += 30;
                }


            }
            else if (_showFormations)
            {
                int y = 130;
                for (int i = 0; i < _formations.Count; i++)
                {
                    Color color = (i == _selectedFormationIndex) ? Color.Yellow : Color.White;

                    spriteBatch.DrawString(_font, $"{_formations[i]}", new Vector2(100, y), color);

                    y += 30;
                }

            }
            else
            {
                for (int i = 0; i < _strings.Count; i++)
                {
                    Color color = (i == _selectionIndex) ? Color.Yellow : Color.White;
                    spriteBatch.DrawString(_font, _strings[i], new Vector2(100, 130 + i * 30), color);
                }
            }

            spriteBatch.DrawString(_font, _gameState.TeamSelected.CurrentFormation.Name, new Vector2(_graphics.GraphicsDevice.Viewport.Width - 300, 100), Color.White, 0f,
                _font.MeasureString(_gameState.TeamSelected.CurrentFormation.Name) / 2, 1.25f, SpriteEffects.None, 0f);
            FormationDrawer.DrawFormation(_gameState.TeamSelected.CurrentFormation, new Vector2(_graphics.GraphicsDevice.Viewport.Width - 600, 100), 1,
                spriteBatch, _textures, _graphics, _font, _gameState, _selectedPositionIndex);

        }
        spriteBatch.End();
    }

    public override void HandleInput(InputState inputState)
    {
        if (_showPlayers)
        {
            if (inputState.IsKeyPressed(Keys.Up))
            {
                if (_selectedPlayerIndex == 0)
                {
                    _selectedPlayerIndex = _orderedList.Count - 1;
                }
                else
                {
                    _selectedPlayerIndex = Math.Max(0, _selectedPlayerIndex - 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Down))
            {
                if (_selectedPlayerIndex == _orderedList.Count - 1)
                {
                    _selectedPlayerIndex = 0;
                }
                else
                {
                    _selectedPlayerIndex = Math.Min(_orderedList.Count - 1, _selectedPlayerIndex + 1);
                }
            }
            if (inputState.IsKeyPressed(Keys.Enter))
            {
                if (_pos != PlayerPositions.NONE)
                {
                    _orderedList = _gameState.TeamSelected.Players
                    .OrderByDescending(p => p.Positions.Max(pos => DataGenerator.PositionCompatibility(pos, _pos)))
                    .ThenBy(p => p.Positions.Min(pos => Math.Abs((int)pos - (int)_pos)))
                    .ThenByDescending(p => p.Overall)
                    .ToList();
                    _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
                        || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));
                    _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_orderedList[_selectedPlayerIndex], _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                    DataGenerator.UpdateLiveOverall(_orderedList[_selectedPlayerIndex], _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                    _pos = PlayerPositions.NONE;
                    _showPlayers = false;
                    _showPositions = true;
                }
                else
                {
                    _orderedList = _gameState.TeamSelected.Players
                        .OrderByDescending(p => p.Overall)
                        .ThenBy(p => p.Positions.First())
                        .ToList();
                    _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
                        || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));
                    _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_orderedList[_selectedPlayerIndex], _selectedPositionIndex - 11);
                    DataGenerator.UpdateLiveOverall(_orderedList[_selectedPlayerIndex], _orderedList[_selectedPlayerIndex].PrimaryPosition);
                    _pos = PlayerPositions.NONE;
                    _showPlayers = false;
                    _showPositions = true;
                }


            }
            if (inputState.IsKeyPressed(Keys.Q))
            {
                _gameState.PlayerSelected = _orderedList[_selectedPlayerIndex];
                ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, "TeamStrategyView"));
                ScreenManager.Instance.ChangeScreen("PlayerView");
            }
            if (inputState.IsKeyPressed(Keys.Escape))
            {
                _showPlayers = false;
                _showPositions = true;
            }
        }
        else if (_showFormations)
        {

            if (inputState.IsKeyPressed(Keys.Up))
            {
                if (_selectedFormationIndex == 0)
                {
                    _selectedFormationIndex = _formations.Count - 1;
                }
                else
                {
                    _selectedFormationIndex = Math.Max(0, _selectedFormationIndex - 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Down))
            {
                if (_selectedFormationIndex == _formations.Count - 1)
                {
                    _selectedFormationIndex = 0;
                }
                else
                {
                    _selectedFormationIndex = Math.Min(_formations.Count - 1, _selectedFormationIndex + 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Enter))
            {

                if (_formations[_selectedFormationIndex] == "4-4-2")
                {
                    Formation formation = new FourFourTwoFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;

                }
                else if (_formations[_selectedFormationIndex] == "4-4-1-1")
                {
                    Formation formation = new FourFourOneOneFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-3-3")
                {
                    Formation formation = new FourThreeThreeFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-3-3 Attacking")
                {
                    Formation formation = new FourThreeThreeAttackingFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-3-3 Defensive")
                {
                    Formation formation = new FourThreeThreeDefensiveFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-1-2-1-2")
                {
                    Formation formation = new FourOneTwoOneTwoFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-2-1-2-1")
                {
                    Formation formation = new FourTwoOneTwoOneFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "4-5-1")
                {
                    Formation formation = new FourFiveOneFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "5-3-2")
                {
                    Formation formation = new FiveThreeTwoFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "5-2-1-2")
                {
                    Formation formation = new FiveTwoOneTwoFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "5-4-1")
                {
                    Formation formation = new FiveFourOneFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "3-4-3")
                {
                    Formation formation = new ThreeFourThreeFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
                else if (_formations[_selectedFormationIndex] == "3-4-1-2")
                {
                    Formation formation = new ThreeFourOneTwoFormation();
                    formation.InitPositions();
                    TransferPlayersToFormation(formation);
                    _gameState.TeamSelected.CurrentFormation = formation;
                }
            }
            if (inputState.IsKeyPressed(Keys.Escape))
            {
                _showFormations = false;
            }
        }
        else if (_showPositions)
        {
            if (inputState.IsKeyPressed(Keys.Up))
            {
                if (_selectedPositionIndex == 0)
                {
                    _selectedPositionIndex = _gameState.TeamSelected.CurrentFormation.Positions.Count + 7 - 1;
                }
                else
                {
                    _selectedPositionIndex = Math.Max(0, _selectedPositionIndex - 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Down))
            {
                if (_selectedPositionIndex == _gameState.TeamSelected.CurrentFormation.Positions.Count + 7 - 1)
                {
                    _selectedPositionIndex = 0;
                }
                else
                {
                    _selectedPositionIndex = Math.Min(_gameState.TeamSelected.CurrentFormation.Positions.Count + 7 - 1, _selectedPositionIndex + 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Enter))
            {
                if (_selectedPositionIndex < 11)
                {
                    _pos = _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex];
                    _orderedList = _gameState.TeamSelected.Players
                    .OrderByDescending(p => p.Positions.Max(pos => DataGenerator.PositionCompatibility(pos, _pos)))
                    .ThenBy(p => p.Positions.Min(pos => Math.Abs((int)pos - (int)_pos)))
                    .ThenByDescending(p => p.Overall)
                    .ToList();
                    _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
                        || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));
                }
                else
                {
                    _pos = PlayerPositions.NONE;
                    _orderedList = _gameState.TeamSelected.Players
                        .OrderByDescending(p => p.Overall)
                        .ThenBy(p => p.Positions.First())
                        .ToList();
                    _orderedList.RemoveAll(item => _gameState.TeamSelected.CurrentFormation.Players.ContainsValue(item)
                        || _gameState.TeamSelected.CurrentFormation.Bench.ContainsValue(item));
                }
                _showPositions = false;
                _showPlayers = true;
            }

            if (inputState.IsKeyPressed(Keys.Q))
            {
                if (_selectedPositionIndex < 11)
                {
                    if (_gameState.TeamSelected.CurrentFormation.Players.ContainsKey(_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]))
                    {
                        _gameState.PlayerSelected = _gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]];
                        ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, "TeamStrategyView"));
                        ScreenManager.Instance.ChangeScreen("PlayerView");
                    }
                }
                else
                {
                    if (_gameState.TeamSelected.CurrentFormation.Bench.ContainsKey(_selectedPositionIndex - 11))
                    {
                        _gameState.PlayerSelected = _gameState.TeamSelected.CurrentFormation.Bench[_selectedPositionIndex - 11];
                        ScreenManager.Instance.AddScreen("PlayerView", new PlayerViewScreen(_gameState, _font, "TeamStrategyView"));
                        ScreenManager.Instance.ChangeScreen("PlayerView");
                    }
                }

            }

            if (inputState.IsKeyPressed(Keys.S))
            {
                if (_selectedPositionIndex < 11)
                {
                    if (_gameState.TeamSelected.CurrentFormation.Players.ContainsKey(_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]))
                    {
                        if (_switcher is null)
                        {
                            _switcher = _gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]];
                            _switcherIndex = _selectedPositionIndex;
                        }
                        else
                        {
                            if (_switcher == _gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]])
                            {
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                            else
                            {
                                if (_switcherIndex < 11)
                                {
                                    _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_gameState.TeamSelected.CurrentFormation
                                        .Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]], _gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);
                                    DataGenerator.UpdateLiveOverall(_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]],
                                        _gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);

                                    _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                    DataGenerator.UpdateLiveOverall(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);

                                    _switcher = null;
                                    _switcherIndex = -1;
                                }
                                else
                                {
                                    _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_gameState.TeamSelected.CurrentFormation
                                        .Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]], _switcherIndex - 11);
                                    DataGenerator.UpdateLiveOverall(_gameState.TeamSelected.CurrentFormation.Bench[_switcherIndex - 11],
                                        _gameState.TeamSelected.CurrentFormation.Bench[_switcherIndex - 11].PrimaryPosition);

                                    _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                    DataGenerator.UpdateLiveOverall(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);

                                    _switcher = null;
                                    _switcherIndex = -1;
                                }
                            }
                        }


                    }
                    else
                    {
                        if (_switcher is not null)
                        {
                            if (_switcherIndex < 11)
                            {
                                _gameState.TeamSelected.CurrentFormation.Players.Remove(_gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);
                                _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                DataGenerator.UpdateLiveOverall(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                            else
                            {
                                _gameState.TeamSelected.CurrentFormation.Bench.Remove(_switcherIndex - 11);
                                _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                DataGenerator.UpdateLiveOverall(_switcher, _gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                        }
                    }
                }
                else
                {
                    if (_gameState.TeamSelected.CurrentFormation.Bench.ContainsKey(_selectedPositionIndex - 11))
                    {
                        if (_switcher is null)
                        {
                            _switcher = _gameState.TeamSelected.CurrentFormation.Bench[_selectedPositionIndex - 11];
                            _switcherIndex = _selectedPositionIndex;
                        }
                        else
                        {
                            if (_switcher == _gameState.TeamSelected.CurrentFormation.Bench[_selectedPositionIndex - 11])
                            {
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                            else
                            {
                                if (_switcherIndex < 11)
                                {
                                    _gameState.TeamSelected.CurrentFormation.AssignPlayerToPosition(_gameState.TeamSelected.CurrentFormation.Bench[_selectedPositionIndex - 11],
                                        _gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);
                                    DataGenerator.UpdateLiveOverall(_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]],
                                        _gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);

                                    _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_switcher, _selectedPositionIndex - 11);
                                    DataGenerator.UpdateLiveOverall(_switcher, _switcher.PrimaryPosition);

                                    _switcher = null;
                                    _switcherIndex = -1;
                                }
                                else
                                {
                                    _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_gameState.TeamSelected.CurrentFormation
                                        .Players[_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]], _switcherIndex - 11);
                                    DataGenerator.UpdateLiveOverall(_gameState.TeamSelected.CurrentFormation.Bench[_switcherIndex - 11],
                                        _gameState.TeamSelected.CurrentFormation.Bench[_switcherIndex - 11].PrimaryPosition);

                                    _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_switcher, _selectedPositionIndex - 11);
                                    DataGenerator.UpdateLiveOverall(_switcher, _switcher.PrimaryPosition);

                                    _switcher = null;
                                    _switcherIndex = -1;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (_switcher is not null)
                        {
                            if (_switcherIndex < 11)
                            {
                                _gameState.TeamSelected.CurrentFormation.Players.Remove(_gameState.TeamSelected.CurrentFormation.Positions[_switcherIndex]);
                                _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_switcher, _selectedPositionIndex - 11);
                                DataGenerator.UpdateLiveOverall(_switcher, _switcher.PrimaryPosition);
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                            else
                            {
                                _gameState.TeamSelected.CurrentFormation.Bench.Remove(_switcherIndex - 11);
                                _gameState.TeamSelected.CurrentFormation.AddPlayerToBench(_switcher, _selectedPositionIndex - 11);
                                DataGenerator.UpdateLiveOverall(_switcher, _switcher.PrimaryPosition);
                                _switcher = null;
                                _switcherIndex = -1;
                            }
                        }
                    }

                }
            }

            if (inputState.IsKeyPressed(Keys.Delete))
            {
                if (_selectedPositionIndex < 11)
                {
                    _gameState.TeamSelected.CurrentFormation.Players.Remove(_gameState.TeamSelected.CurrentFormation.Positions[_selectedPositionIndex]);
                }
                else
                {
                    _gameState.TeamSelected.CurrentFormation.Bench.Remove(_selectedPositionIndex - 11);
                }

            }

            if (inputState.IsKeyPressed(Keys.Escape))
            {
                _showPositions = false;
            }
        }
        else
        {
            if (inputState.IsKeyPressed(Keys.Up))
            {
                if (_selectionIndex == 0)
                {
                    _selectionIndex = _strings.Count - 1;
                }
                else
                {
                    _selectionIndex = Math.Max(0, _selectionIndex - 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Down))
            {
                if (_selectionIndex == _strings.Count - 1)
                {
                    _selectionIndex = 0;
                }
                else
                {
                    _selectionIndex = Math.Min(_strings.Count - 1, _selectionIndex + 1);
                }
            }

            if (inputState.IsKeyPressed(Keys.Enter))
            {
                if (_selectionIndex == 0)
                {
                    _showFormations = true;
                }
                else if (_selectionIndex == 1)
                {
                    _showPositions = true;
                }
            }

            if (inputState.IsKeyPressed(Keys.Escape))
            {
                _gameState.TeamSelected = null;
                ScreenManager.Instance.ChangeScreen("CareerMenu");
            }
        }
    }

    public void TransferPlayersToFormation(Formation formation) {

            for (int i = 0; i < formation.Positions.Count; i++)
            {
                if (_gameState.TeamSelected.CurrentFormation.Players.ContainsKey(_gameState.TeamSelected.CurrentFormation.Positions[i]))
                {
                    formation.Players[formation.Positions[i]] = _gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]];
                    DataGenerator.UpdateLiveOverall(_gameState.TeamSelected.CurrentFormation.Players[_gameState.TeamSelected.CurrentFormation.Positions[i]], formation.Positions[i]);
                }
            }
            for (int i = 0; i < 7; i++)
            {
                if (_gameState.TeamSelected.CurrentFormation.Bench.ContainsKey(i))
                {
                    formation.Bench[i] = _gameState.TeamSelected.CurrentFormation.Bench[i];
                    DataGenerator.UpdateLiveOverall(formation.Bench[i], formation.Bench[i].PrimaryPosition);
                }
            }
        }
    
}


