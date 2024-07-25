using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.States
{
    public class CombatState : IEngineState, ICombatListener
    {
        private readonly Combat _combat;
        private int _selectedOption;
        private bool _combatEnded;
        public CombatState(Combat combat)
        {
            _combat = combat;
            _combat.AddListener(this);
        }
        public void Activate()
        {
            Render();
        }
        private void RenderHeader()
        {
            Console.Clear();
            Console.WriteLine("You (hp: {0}) vs {1} (hp: {2})", _combat.Player.Hp, _combat.Entity.Name, _combat.Entity.Hp);
            Console.WriteLine("--------------------------------------");
        }
        private void Render()
        {
            var optionCount = _combat.Player.Abilities.Count() + _combat.Player.Inventory.Where(x => x is IUsableItem).Count();
            //Corrects the _selectedOption index when an item with only 1 charge is used an removed from the Inventory of the player
            if (_selectedOption > optionCount)
            {
                _selectedOption = optionCount - 1;
            }
            RenderHeader();
            Console.WriteLine("Abilities:");
            var index = 0;

            foreach (var ability in _combat.Player.Abilities)
            {
                var abilityDamage = ability.GetDamage().Amount;
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                Console.WriteLine($"{ability.Name} - {abilityDamage + _combat.Player.DamageBuff} Damage");
                index++;
                ColorConsole(false);
            }
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Items:");
            foreach (IUsableItem item in _combat.Player.Inventory.Where(x => x is IUsableItem))
            {
                var itemDamage = item.GetDamage();
                var effect = item.GetEffect();
                var modifier = item.GetAmount();
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                Console.WriteLine($"{item.Name}[{item.Charges}] - {modifier} {effect}");
                
                index++;
                ColorConsole(false);
            }
            Console.WriteLine("--------------------------------------");
            if (_selectedOption == optionCount)
            {
                ColorConsole(true);
            }
            Console.WriteLine("Run");
            ColorConsole(false);
            Console.WriteLine("--------------------------------------");
        }

        public void Deactivate()
        {

        }

        public void DisplayMessage(string message)
        {
            RenderHeader();
            Console.WriteLine(message);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        public void Dispose()
        {
            _combat.RemoveListener(this);
        }

        public void EndCombat()
        {
            _combatEnded = true;
            _combat.Player.ResetBuff();
            Program.Engine.PopState(this);
        }

        public void PlayerDied()
        {
            Console.WriteLine("You Died!");
            Console.ReadKey();
            Program.Engine.Quit();
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            var abilityCount = _combat.Player.Abilities.Count();
            var itemCount = _combat.Player.Inventory.Where(x => x is IUsableItem).Count();
            var totalCount = abilityCount + itemCount;
            if (key.Key == ConsoleKey.W)
            {
                if (_selectedOption > 0)
                {
                    _selectedOption--;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedOption < totalCount + 1)
                {
                    _selectedOption++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (_selectedOption < abilityCount)
                {
                    _combat.UseAbility(_combat.Player.Abilities.ElementAt(_selectedOption));
                }
                else if (_selectedOption > abilityCount - 1 && _selectedOption < totalCount)
                {
                    _combat.UseItem(_combat.Player.Inventory.Where(x => x is IUsableItem).ElementAt(_selectedOption - abilityCount) as IUsableItem);
                }
                else
                {
                    _combat.Run();
                }
            }
            if (!_combatEnded)
            {
                Render(); 
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
