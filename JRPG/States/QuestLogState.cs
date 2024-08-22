using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.States
{
    public class QuestLogState : IEngineState
    {
        private readonly Entity _player;
        private int _selectedQuest;
        public QuestLogState(Entity player)
        {
            _player = player;
        }
        public void Activate()
        {
            Render();
        }

        public void Deactivate()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Program.Engine.PopState(this);
                return;
            }
        }
        private void Render()
        {
            Console.Clear();
            Console.WriteLine("Quest Log");
            Console.WriteLine("-------------------------------------");
            var questIndex = 0;
            foreach (var questLine in _player.GetComponent<PlayerComponent>().Player.ActiveQuestLines)
            {
                foreach (var quest in questLine.RemainingQuests)
                {
                    if (questIndex == _selectedQuest)
                    {
                        ColorConsole(true);
                    }
                    if (quest.State == QuestState.Active)
                    {
                        var killQuest = quest as KillQuest;
                        if (killQuest != null)
                        {
                            Console.WriteLine($"{killQuest.Title} - {killQuest.Reward.Name} - {killQuest.EnemiesDefeated}/{killQuest.NumberOfEnemiesToDestroy}");
                        }
                        else if (quest.Reward != null)
                        {
                            Console.WriteLine($"{quest.Title} - {quest.Reward.Name}, {quest.Experience} XP - In Progress");
                        }
                        else
                        {
                            Console.WriteLine($"{quest.Title} - {quest.Experience} XP - In Progress");
                        }
                    }
                    if (quest.State == QuestState.Completed)
                    {
                        if (quest.Reward != null)
                        {
                            Console.WriteLine($"{quest.Title} - {quest.Reward.Name}, {quest.Experience} XP - Done");
                        }
                        else
                        {
                            Console.WriteLine($"{quest.Title} - {quest.Experience} XP - Done");
                        }
                    }
                    ColorConsole(false);
                }
                questIndex++;
            }
            Console.WriteLine("-------------------------------------");
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
