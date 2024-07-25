using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.Models
{
    public class Player
    {
        //TODO: Armor slot system
        private readonly List<IEquippableItem> _equippedItems;
        private readonly List<IItem> _inventory;
        private readonly List<IAbility> _abilities;

        public IEnumerable<IEquippableItem> EquippedItems { get { return _equippedItems; } }
        public IEnumerable<IItem> Inventory { get { return _inventory; } }
        public IEnumerable<IAbility> Abilities { get { return _abilities; } }
        public int Hp { get; private set; }
        public int Gold { get; private set; }
        public int AttackSpeed { get; private set; }
        public Player() 
        {
            _equippedItems = new List<IEquippableItem>();
            _inventory = new List<IItem>();
            _abilities = new List<IAbility>();
            Hp = 200;
            Gold = 100;
            AttackSpeed = 10;
        }
        public void AddAbility(IAbility ability)
        {
            _abilities.Add(ability);
        }
        public void AddItem (IItem item)
        {
            _inventory.Add(item);
        }
        public void RemoveItem (IItem item)
        {
            _inventory.Remove(item);
        }
        public bool EquipItem (IEquippableItem item)
        {
            if (!_equippedItems.Any(x => string.Equals(x.Name,item.Name,StringComparison.CurrentCultureIgnoreCase)))
            {
                _inventory.Remove(item);
                _equippedItems.Add(item);
                UpdateStats();
                return true;
            }
            return false;
        }
        public void UnEquipItem (IEquippableItem item)
        {
            _equippedItems.Remove(item);
            _inventory.Add(item);
            UpdateStats();
        }

        public void TakeDamage(Damage damage)
        {
            foreach (var item in EquippedItems)
            {
                item.ModifyDamage(damage);
            }
            Hp -= damage.Amount;
            // damage = _equippedItems.Aggregate(damage, (a, i) => i.ModifyDamage(a));
        }
        public void SpendGold(int input)
        {
            Gold -= input;
        }
        public void ReceiveGold(int input)
        {
            Gold += input;
        }
        public void UpdateStats()
        {
            foreach(IEquippableItem item in EquippedItems)
            {
                if (item.Weight > 0)
                {
                    AttackSpeed -= item.Weight;
                }
            }
        }
    }
}
