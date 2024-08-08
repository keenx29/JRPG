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
            foreach (var questLine in _player.GetComponent<PlayerComponent>().Player.Quests)
            {
                foreach (var quest in questLine.Quests)
                {
                    if (questIndex == _selectedQuest)
                    {
                        ColorConsole(true);
                    }
                    Console.WriteLine($"{quest.Title} - {quest.Reward.Name} - {(quest.Requirement(_player) == true ? "Done" : "In Progress")}");
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
