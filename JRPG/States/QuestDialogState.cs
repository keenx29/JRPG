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
        private readonly QuestChannel _questChannel;
        private bool _questInspectScreen = false;
        private bool _questDeliverScreen = false;
        private int _dialogHeight;
        private int _selectedOption;
        private IQuestLine _currentQuestLine;
        private IQuest _currentQuest;
        private List<Tuple<string, IDialogScreen>> _optionList;
        public QuestDialogState(IQuestDialog dialog, Entity instigator,QuestChannel questChannel)
        {
            _dialog = dialog;
            _instigator = instigator;
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
            foreach (var questLine in _dialog.QuestLines)
            {
                if (_player.ActiveQuestLines.Contains(questLine, new QuestLineComparer()))
                {
                    _currentQuestLine = questLine;
                    if (_currentQuestLine.RemainingQuests.FirstOrDefault()?.State == QuestState.Active)
                    {
                        _currentQuest = _currentQuestLine.RemainingQuests.First();
                        RenderQuestInProgress(_currentQuestLine);
                        return;
                    }
                    else if (_currentQuestLine.RemainingQuests.FirstOrDefault()?.State == QuestState.Completed)
                    {
                        _questDeliverScreen = true;
                        _currentQuest = _currentQuestLine.RemainingQuests.First();
                        RenderQuestInProgress(_currentQuestLine);
                        return;
                    }
                }
            }
            RenderAvailableQuestLines();
        }

        private void RenderAvailableQuestLines()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------");
            var index = 0;
            foreach (var questLine in _dialog.QuestLines)
            {
                if (_player.CompletedQuestLines.Contains(questLine,new QuestLineComparer()))
                {
                    _dialog.QuestLines.Remove(questLine);
                }
            }
            foreach (var questLine in _dialog.QuestLines)
            {
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                Console.WriteLine($"{questLine.Name} - {questLine.RemainingQuests.First().Title}");
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
            Console.WriteLine(_currentQuestLine.Name) ;
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(_currentQuest.Title);
            Console.WriteLine("-------------------------------------------");
            _dialogHeight = Console.CursorTop;
            var index = 0;
            var killQuest = _currentQuest as KillQuest;
            var deliveryQuest = _currentQuest as DeliveryQuest;
            if (deliveryQuest != null)
            {
                if (deliveryQuest.State == QuestState.Pending)
                {
                    Console.WriteLine($"{deliveryQuest.Description}");
                    Console.WriteLine("-------------------------------------------");
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
            }
            if (killQuest != null)
            {
                if (killQuest.State == QuestState.Pending)
                {
                    Console.WriteLine($"{killQuest.Description}");
                    Console.WriteLine($"Slay: {killQuest.NumberOfEnemiesToDestroy} {(killQuest.NumberOfEnemiesToDestroy == 1 ? killQuest.EnemyToDefeat.Name : killQuest.EnemyToDefeat.Name + "s")}");
                    Console.WriteLine("-------------------------------------------");
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
                else if (killQuest.State == QuestState.Active)
                {
                    Console.WriteLine($"{killQuest.Description}");
                    Console.WriteLine($"{killQuest.Description}");
                }
            }
            if (_currentQuest.State == QuestState.Completed)
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
                        rewardString += $" {reward.GetAmount()} {reward.GetEffect()}";
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
                    Console.WriteLine("Close!");
                }else
                {
                    Console.WriteLine("Complete!");
                    ColorConsole(true);
                    Console.WriteLine("Close!");
                    ColorConsole(false);
                    
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
                if (_questInspectScreen || _questDeliverScreen)
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
                        //If the player accepted the quest start it and exit the state
                        _questChannel.DeliverQuest(_currentQuest);
                        Program.Engine.PopState(this);
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
                if (_questInspectScreen)
                {
                    if (_selectedOption == 0)
                    {
                        //If the player accepted the quest start it and exit the state
                        _questChannel.AssignQuestLine(_currentQuestLine);
                        Program.Engine.PopState(this);
                    }
                    else
                    {
                        //If the player denies the quest take him back to the available quest lines window
                        _selectedOption = 0;
                        _questInspectScreen = false;
                        RenderAvailableQuestLines();
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
                    _questInspectScreen = true;
                    _selectedOption = 0;
                    RenderQuestInProgress(_currentQuestLine);

                }
                
                //Start the selected QuestLine
                return;
            }
            if (_questInspectScreen || _questDeliverScreen)
            {
                RenderQuestInProgress(_currentQuestLine);
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
