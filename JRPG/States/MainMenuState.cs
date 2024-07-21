using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.States
{
    class MainMenuState : IEngineState
    {
        private readonly Player _player;
        public MainMenuState(Player player)
        {
            _player = player;
        }

        public void Activate()
        {
            Console.Clear();
            Console.WriteLine("IN MAIN MENU STATE - press Enter for inventory");
            Console.WriteLine("Enter for inventory");
            Console.WriteLine("ESC to go back");
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
            }
            if (key.Key == ConsoleKey.Enter)
            {
                Program.Engine.PushState(new InventoryState(_player));
            }
        }
    }
}
