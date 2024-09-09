using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.States
{
    public class QuestDialogState : IEngineState
    {
        private readonly IQuestDialog _dialog;
        private readonly Entity _instigator;
        private readonly Player _player;
        private readonly Entity _npc;
        private readonly QuestChannel _questChannel;
        private bool _questAcceptScreen = false;
        private bool _questDeliverScreen = false;
        private bool _questInspectScreen = false;
        private int _dialogHeight;
        private int _selectedOption;
        private IQuestLine _currentQuestLine;
        private IQuest _currentQuest;
        private List<Tuple<string, IDialogScreen>> _optionList;
        public QuestDialogState(IQuestDialog dialog, Entity instigator,Entity npc,QuestChannel questChannel)
        {
            _dialog = dialog;
            _instigator = instigator;
            _npc = npc;
            _player = _instigator.GetComponent<PlayerComponent>().Player;
            //_currentScreen = _dialog.Screens.First();
            _questChannel = questChannel;
            //SwitchScreen(_dialog.FirstScreen);
        }
        private void QuestCompletedEvent(IQuest quest)
        {
            //Remove the quest from the questLine 
        }
        public void Activate()
        {
            //Check if the player has any of the questLines active and grab it in a var
            //foreach (var questLine in _dialog.QuestLines)
            //{
            //    if (_player.ActiveQuestLines.Contains(questLine, new QuestLineComparer()))
            //    {
            //        _currentQuestLine = questLine;
            //        _currentQuest = _currentQuestLine.RemainingQuests.First();
            //        if (_currentQuest.State == QuestState.Active)
            //        {
            //            RenderQuestInProgress(_currentQuestLine);
            //            return;
            //        }
            //        else if (_currentQuestLine.RemainingQuests.FirstOrDefault()?.State == QuestState.Completed)
            //        {
            //            if (_currentQuest.EndPoint == _npc)
            //            {
            //                _questDeliverScreen = true;
            //                RenderQuestInProgress(_currentQuestLine);
            //                return;
            //            }
            //        }
            //    }
            //}
            RenderAvailableQuestLines();
        }

        private void RenderAvailableQuestLines()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            var index = 0;
            foreach (var questLine in _dialog.QuestLines.ToList())
            {
                if (_player.CompletedQuestLines.Contains(questLine,new QuestLineComparer()))
                {
                    _dialog.QuestLines.Remove(questLine);
                }
            }
            foreach (var questLine in _dialog.QuestLines)
            {
                var nextQuest = questLine.RemainingQuests.FirstOrDefault();
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                if (nextQuest != null)
                {
                    if (nextQuest.State == QuestState.Active && nextQuest is KillQuest nextKillQuest)
                    {
                        Console.WriteLine($"{questLine.Name} - {nextKillQuest.Title} - {nextKillQuest.EnemiesDefeated}/{nextKillQuest.NumberOfEnemiesToDestroy}");
                        
                    }
                    else if (nextQuest.State == QuestState.Completed )
                    {
                        Console.WriteLine($"{questLine.Name} - {nextQuest.Title} - Done");
                        
                    }
                    else if (nextQuest.State == QuestState.Pending && nextQuest.StartPoint == _npc)
                    {
                        Console.WriteLine($"{questLine.Name} - {questLine.RemainingQuests.First().Title} - Inspect");

                    }
                    else
                    {
                        continue;
                    }
                }
                ColorConsole(false);
                Console.WriteLine("-------------------------------------------");
                index++;
            }
            if (_selectedOption == index)
            {
                ColorConsole(true);
            }
            Console.WriteLine("Close!");
            ColorConsole(false);
        }
            

        private void RenderQuestInProgress(IQuestLine currentQuestLine)
        {
            Console.Clear();
            Console.WriteLine($"Questline - {_currentQuestLine.Name}") ;
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Quest - {_currentQuest.Title}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(_currentQuest.Description);
            Console.WriteLine("-------------------------------------------");
            _dialogHeight = Console.CursorTop;
            var index = 0;
            var killQuest = _currentQuest as KillQuest;
            var deliveryQuest = _currentQuest as DeliveryQuest;
            if (deliveryQuest != null)
            {
                if (deliveryQuest.State == QuestState.Pending)
                {
                    if (deliveryQuest.StartPoint == _npc)
                    {
                        _questAcceptScreen = true;
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine("Accept!");
                            ColorConsole(false);
                            Console.WriteLine("Deny!");
                        }
                        else
                        {
                            Console.WriteLine("Accept!");
                            ColorConsole(true);
                            Console.WriteLine("Deny!");
                            ColorConsole(false);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"This quest starts from {deliveryQuest.StartPoint.GetComponent<SpriteComponent>().Sprite}");
                        Console.WriteLine("-------------------------------------------");
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine($"Go Back!");
                            ColorConsole(false);
                            Console.WriteLine($"Close!");
                        }
                        else
                        {
                            Console.WriteLine($"Go Back!");
                            ColorConsole(true);
                            Console.WriteLine($"Close!");
                            ColorConsole(false);
                        }
                        _questInspectScreen = true;
                    }
                }
                else if (deliveryQuest.State == QuestState.Completed)
                {
                    if (deliveryQuest.EndPoint == _npc)
                    {
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine("Complete!");
                            ColorConsole(false);
                            Console.WriteLine("Go Back!");
                        }
                        else
                        {
                            Console.WriteLine("Complete!");
                            ColorConsole(true);
                            Console.WriteLine("Go Back!");
                            ColorConsole(false);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"This quest ends at {deliveryQuest.StartPoint.GetComponent<SpriteComponent>().Sprite}");
                        Console.WriteLine("-------------------------------------------");
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine($"Go Back!");
                            ColorConsole(false);
                            Console.WriteLine($"Close!");
                        }
                        else
                        {
                            Console.WriteLine($"Go Back!");
                            ColorConsole(true);
                            Console.WriteLine($"Close!");
                            ColorConsole(false);
                        }
                        _questInspectScreen = true;
                    }
                }
            }
            if (killQuest != null)
            {
                if (killQuest.State == QuestState.Pending)
                {
                    Console.WriteLine($"Slay: {killQuest.NumberOfEnemiesToDestroy} {(killQuest.NumberOfEnemiesToDestroy == 1 ? killQuest.EnemyToDefeat.Name : killQuest.EnemyToDefeat.Name + "s")}");
                    Console.WriteLine("-------------------------------------------");
                    if (killQuest.StartPoint == _npc)
                    {
                        _questAcceptScreen = true;
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine("Accept!");
                            ColorConsole(false);
                            Console.WriteLine("Deny!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Accept!");
                            ColorConsole(true);
                            Console.WriteLine("Deny!");
                            ColorConsole(false);
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"This quest starts from {deliveryQuest.StartPoint.GetComponent<SpriteComponent>().Sprite}");
                        Console.WriteLine("-------------------------------------------");
                        if (_selectedOption == index)
                        {
                            ColorConsole(true);
                            Console.WriteLine($"Go Back!");
                            ColorConsole(false);
                            Console.WriteLine($"Close!");
                        }
                        else
                        {
                            Console.WriteLine($"Go Back!");
                            ColorConsole(true);
                            Console.WriteLine($"Close!");
                            ColorConsole(false);
                        }
                        _questInspectScreen = true;
                    }
                }
                else if (killQuest.State == QuestState.Active)
                {
                    Console.WriteLine($"Progress: {killQuest.NumberOfEnemiesToDestroy}/{killQuest.EnemiesDefeated} {(killQuest.NumberOfEnemiesToDestroy == 1 ? killQuest.EnemyToDefeat.Name : killQuest.EnemyToDefeat.Name + "s")}");
                    Console.WriteLine("-------------------------------------------");
                    if (_selectedOption == index)
                    {
                        ColorConsole(true);
                        Console.WriteLine($"Go Back!");
                        ColorConsole(false);
                        Console.WriteLine($"Close!");
                    }
                    else
                    {
                        Console.WriteLine($"Go Back!");
                        ColorConsole(true);
                        Console.WriteLine($"Close!");
                        ColorConsole(false);
                    }
                    
                    _questInspectScreen = true;
                    return;
                }
            }
            if (_currentQuest.State == QuestState.Completed)
            {
                if (_currentQuest.EndPoint == _npc)
                {
                    _questDeliverScreen = true;
                    Console.WriteLine($"Thanks for the help. Here is your reward:");
                    if (_currentQuest.Reward != null)
                    {
                        var rewardString = $"{_currentQuest.Reward.Name}";
                        if (_currentQuest.Reward is Gear)
                        {
                            var reward = _currentQuest.Reward as Gear;
                            rewardString += $" {reward.Attack} Damage, {reward.Defense} Defense, {reward.Weight} kgs";
                        }
                        else if (_currentQuest.Reward is Consumable)
                        {
                            var reward = _currentQuest.Reward as Consumable;
                            rewardString += $"[{reward.Charges}] {reward.GetAmount()} {reward.GetEffect()}";
                        }
                        Console.WriteLine(rewardString);
                    }
                    Console.WriteLine($"{_currentQuest.Experience} XP");
                    Console.WriteLine("-------------------------------------------");
                    if (_selectedOption == index)
                    {
                        ColorConsole(true);
                        Console.WriteLine("Complete!");
                        ColorConsole(false);
                        Console.WriteLine("Go Back!!");
                    }
                    else
                    {
                        Console.WriteLine("Complete!");
                        ColorConsole(true);
                        Console.WriteLine("Go Back!!");
                        ColorConsole(false);

                    }
                }
                else
                {

                }
            }
        }

        public void Deactivate()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.W)
            {
                if (_selectedOption > 0)
                {
                    _selectedOption--;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_questAcceptScreen || _questDeliverScreen || _questInspectScreen)
                {
                    if (_selectedOption < 1)
                    {
                        _selectedOption++;
                    }
                }
                else if (_selectedOption < _dialog.QuestLines.Count())
                {
                    _selectedOption++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                //Player selected a QuestLine and Quest
                if (_questDeliverScreen) 
                {
                    if (_selectedOption == 0)
                    {
                        //If the player delivers the quest finish the quest and exit the state
                        _questChannel.DeliverQuest(_currentQuest);
                        Program.Engine.PopState(this);
                        return;
                    }
                    else
                    {
                        //If the player denies delivering the quest take him back to the available quest lines window
                        _selectedOption = 0;
                        _questDeliverScreen = false;
                        RenderAvailableQuestLines();
                        return;
                    }
                }
                else if (_questAcceptScreen)
                {
                    if (_selectedOption == 0)
                    {
                        //If the player accepted the quest start it and exit the state
                        _questChannel.AssignQuestLine(_currentQuestLine);
                        Program.Engine.PopState(this);
                        return;
                    }
                    else
                    {
                        //If the player denies the quest take him back to the available quest lines window
                        _selectedOption = 0;
                        _questAcceptScreen = false;
                        RenderAvailableQuestLines();
                        return;
                    }
                }
                else if (_questInspectScreen)
                {
                    if (_selectedOption == 0)
                    {
                        //If the player selected to Go Back to all available quests screent
                        _questInspectScreen = false;
                        RenderAvailableQuestLines();
                        return;
                    }
                    else
                    {
                        //If the player chooses to close the window
                        _selectedOption = 0;
                        _questAcceptScreen = false;
                        Program.Engine.PopState(this);
                        return;
                    }
                }
                else if (_selectedOption == _dialog.QuestLines.Count()) 
                {
                    //Player chooses to Close the windows without selecting a Quest and QuestLine
                    Program.Engine.PopState(this);
                    return;
                }
                else
                {
                    _currentQuestLine = _dialog.QuestLines[_selectedOption];
                    _currentQuest = _currentQuestLine.RemainingQuests.First();
                    _selectedOption = 0;
                    RenderQuestInProgress(_currentQuestLine);

                }
                return;
            }
            //Rerender the screen to update what the player is selecting with the W and S keys
            if (_questAcceptScreen || _questDeliverScreen || _questInspectScreen)
            {
                RenderQuestInProgress(_currentQuestLine);
                return;
            }
            else
            {
                RenderAvailableQuestLines();
            }
        }
        private void ColorConsole(bool selected)
        {
            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
