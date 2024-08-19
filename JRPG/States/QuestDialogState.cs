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
            if (killQuest != null)
            {
                if (killQuest.State == QuestState.Active)
                {
                    Console.WriteLine($"Progress: {killQuest.EnemiesDefeated}/{killQuest.NumberOfEnemiesToDestroy}");
                    Console.WriteLine($"Good luck!");
                    Console.WriteLine("-------------------------------------------");
                    ColorConsole(true);
                    Console.WriteLine("Close!");
                    ColorConsole(false);
                }
                else if (killQuest.State == QuestState.Completed)
                {
                    Console.WriteLine($"Thanks for the help. Here is your reward:");
                    if (killQuest.Reward != null)
                    {
                        var rewardString = $"{killQuest.Reward.Name}";
                        if (killQuest.Reward is Gear)
                        {
                            var reward = killQuest.Reward as Gear;
                            rewardString += $" {reward.Attack} Damage, {reward.Defense} Defense, {reward.Weight} kgs";
                        }
                        else if (killQuest.Reward is Consumable)
                        {
                            var reward = killQuest.Reward as Consumable;
                            rewardString += $" {reward.GetAmount()} {reward.GetEffect()}";
                        }
                        Console.WriteLine(rewardString);
                    }
                    Console.WriteLine($"{killQuest.Experience} XP");
                    Console.WriteLine("-------------------------------------------");
                    ColorConsole(true );
                    Console.WriteLine("Complete!");
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
                if (_selectedOption < _dialog.QuestLines.Count())
                {
                    _selectedOption++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (_selectedOption == _dialog.QuestLines.Count()) 
                {
                    Program.Engine.PopState(this);
                    return;
                }
                else
                {
                    var selectedQuestLine = _dialog.QuestLines[_selectedOption];
                    var selectedQuest = selectedQuestLine.RemainingQuests.First();
                    _questChannel.AssignQuest(selectedQuest);
                    _currentQuest = selectedQuest;
                    _currentQuestLine = selectedQuestLine;
                    RenderQuestInProgress(selectedQuestLine);
                }
                
                //Start the selected QuestLine
                return;
            }
            RenderAvailableQuestLines();
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
