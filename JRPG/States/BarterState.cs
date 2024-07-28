using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.States
{
    public class BarterState : IEngineState, IBarterListener
    {
        private readonly Barter _barter;

        private int _selectedOption;
        private bool _barteringEnded;
        public BarterState(Barter barter)
        {
            _barter = barter;
        }
        private void RenderHeader()
        {
            Console.Clear();
            Console.WriteLine($"Bartering with {_barter.Entity.Name}.");
            Console.WriteLine("--------------------------------------");
        }
        private void Render()
        {
            RenderHeader();
            Console.WriteLine("For Sale:");
            var index = 0;

            foreach (var item in _barter.Entity.Inventory)
            {
                var equippableItem = item as IEquippableItem;
                var usableItem = item as IUsableItem;
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                if (equippableItem != null)
                {
                    Console.WriteLine($"{equippableItem.Name} - {equippableItem.Defense} Defense - {_barter.Entity.GetBuyPrice(equippableItem)}g");
                }
                else if (usableItem != null)
                {
                    var effect = usableItem.GetEffect();
                    var modifier = usableItem.GetAmount();
                    switch (effect)
                    {
                        case "attack":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Damage - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "protect":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Defense - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "heal":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Health - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "buff":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Damage Bonus - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        default:
                            Console.WriteLine($"{usableItem.Name} - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"{item.Name} - {_barter.Entity.GetBuyPrice(item)}g");
                }
                
                index++;
                ColorConsole(false);
            }
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Inventory:");
            foreach (var item in _barter.Player.Inventory)
            {
                var equippableItem = item as IEquippableItem;
                var usableItem = item as IUsableItem;
                if (_selectedOption == index)
                {
                    ColorConsole(true);
                }
                if (equippableItem != null)
                {
                    if (equippableItem is Gear)
                    {
                        Console.WriteLine($"{equippableItem.Name} - {equippableItem.Defense} Defense - {_barter.Entity.GetSalePrice(equippableItem)}g");
                    }
                    else if (equippableItem is Weapon)
                    {
                        Console.WriteLine($"{equippableItem.Name} - {equippableItem.Defense} Damage - {_barter.Entity.GetSalePrice(equippableItem)}g");
                    }
                }
                else if (usableItem != null)
                {
                    var effect = usableItem.GetEffect();
                    var modifier = usableItem.GetAmount();
                    switch (effect)
                    {
                        case "attack":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Damage - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "protect":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Defense - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "heal":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Health - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        case "buff":
                            Console.WriteLine($"{usableItem.Name}[{usableItem.Charges}] - {modifier} Damage Bonus - {_barter.Entity.GetBuyPrice(usableItem)}g");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine($"{item.Name} - {_barter.Entity.GetSalePrice(item)}g");
                index++;
                ColorConsole(false);
            }
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Balance: {_barter.Player.Gold}g");

            // TODO: Introduce selling and bartering item for item instead of just buying

            //foreach (var item in _barter.Player.Inventory.Where(x => x.CanUse))
            //{
            //    if (_selectedOption == index)
            //    {
            //        ColorConsole(true);
            //    }
            //    Console.WriteLine(item.Name);
            //    index++;
            //    ColorConsole(false);
            //}
        }
        public void Activate()
        {
            var itemList = _barter.Player.Inventory.Concat(_barter.Entity.Inventory).ToList();
            _barter.Entity.GeneratePrices(itemList);
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
            var traderItemCount = _barter.Entity.Inventory.Count();
            var playerItemCount = _barter.Player.Inventory.Count();
            var totalCount = traderItemCount + playerItemCount;
            if (key.Key == ConsoleKey.W)
            {
                if (_selectedOption > 0)
                {
                    _selectedOption--;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedOption < totalCount - 1)
                {
                    _selectedOption++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (_selectedOption < traderItemCount)
                {
                    _barter.BuyItem(_barter.Entity.Inventory.ElementAt(_selectedOption));
                    //_combat.UseAbility(_combat.Player.Abilities.ElementAt(_selectedOption));
                }
                else if (_selectedOption > traderItemCount - 1)
                {
                    _barter.SellItem(_barter.Player.Inventory.ElementAt(_selectedOption - traderItemCount));
                    //_combat.UseItem(_combat.Player.Inventory.Where(x => x.CanUse).ElementAt(_selectedOption - abilityCount));
                }
            }
            if (!_barteringEnded)
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

        public void EndBarter()
        {
            _barteringEnded = true;
            Program.Engine.PopState(this);
        }
    }
}
