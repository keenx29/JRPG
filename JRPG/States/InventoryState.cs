using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.States
{
    public class InventoryState : IEngineState
    {
        private readonly Player _player;
        private int _selectedItem;
        public InventoryState(Player player)
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
            var equippedCount = _player.EquippedItems.Count();
            var inventoryCount = _player.Inventory.Count();
            var totalItemCount = equippedCount + inventoryCount;
            if (key.Key == ConsoleKey.Escape)
            {
                Program.Engine.PopState(this);
                return;
            }
            else if (key.Key == ConsoleKey.W)
            {
                if (_selectedItem > 0)
                {
                    _selectedItem--;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedItem < totalItemCount - 1)
                {
                    _selectedItem++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (_selectedItem < equippedCount)
                {
                    var itemToUnequip = _player.EquippedItems.ElementAt(_selectedItem);
                    _player.UnEquipItem(itemToUnequip);
                }else
                {
                    var itemToEquip = _player.Inventory.ElementAt(_selectedItem - equippedCount);
                    if (itemToEquip is IEquippableItem)
                    {
                        if (!_player.EquipItem(itemToEquip as IEquippableItem))
                        {
                            Render($"{itemToEquip.Name} is already equipped!");
                            return;
                        }

                    }
                    else
                    {
                        Render($"{itemToEquip.Name} is unequippable!");
                        return;
                    }
                }
            }
            Render();
        }
        private void Render()
        {
            Render(null);
        }
        private void Render(string message)
        {
            Console.Clear();
            Console.WriteLine("Player Inventory");
            Console.WriteLine("-------------------------------------");
            var itemIndex = 0;
            var damageModifier = 0;
            foreach (var item in _player.EquippedItems)
            {
                if (itemIndex == _selectedItem)
                {
                    ColorConsole(true);
                }
                if (item is Gear)
                {
                    damageModifier = item.GetDefenseModifier();
                    Console.WriteLine($"[*] {item.Name} - {(damageModifier)} Defense");
                }
                else if (item is Weapon)
                {
                    damageModifier = item.GetDamageModifier();
                    Console.WriteLine($"[*] {item.Name} - {damageModifier} Damage");
                }
                else
                {
                    Console.WriteLine($"[*] {item.Name}");
                }

                ColorConsole(false);

                itemIndex++;
            }
            Console.WriteLine("-------------------------------------");
            foreach (var item in _player.Inventory)
            {
                if (itemIndex == _selectedItem)
                {
                    ColorConsole(true);
                }
                var equippableItem = item as IEquippableItem;
                var usableItem = item as IUsableItem;
                if (equippableItem != null)
                {
                    if (equippableItem is Gear)
                    {
                        damageModifier = equippableItem.GetDefenseModifier();
                        Console.WriteLine($"[ ]{equippableItem.Name} - {damageModifier} Defense");
                    }
                    else if (equippableItem is Weapon)
                    {
                        damageModifier = equippableItem.GetDamageModifier();
                        Console.WriteLine($"[ ]{equippableItem.Name} - {damageModifier} Damage");
                    }
                }
                else if (usableItem != null)
                {
                    var effect = usableItem.GetEffect();
                    damageModifier = usableItem.GetAmount();
                    Console.WriteLine($"{usableItem.Name} - {damageModifier} {effect}");
                }
                ColorConsole(false);

                itemIndex++;
            }
            Console.WriteLine("-------------------------------------");
            if (message != null)
            {
                Console.WriteLine(message);
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
