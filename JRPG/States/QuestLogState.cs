using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.States
{
    public class QuestLogState : IEngineState
    {
        private readonly Player _player;
        private int _selectedQuest;
        public QuestLogState(Player player)
        {
            _player = player;
        }
        public void Activate()
        {
            Render();
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            
        }
        private void Render()
        {
            Console.Clear();
            Console.WriteLine("Quest Log");
            Console.WriteLine("-------------------------------------");
            var questIndex = 0;
            foreach (var questLine in _player.Quests)
            {
                foreach (var quest in questLine.Quests)
                {
                    if (questIndex == _selectedQuest)
                    {
                        ColorConsole(true);
                    }
                    Console.WriteLine($"{quest.Title} - {quest.Reward.Name}");
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
