using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Player
    {
        private readonly List<IItem> _equippedItems;
        private readonly List<IItem> _inventory;
        private readonly List<IAbility> _abilities;

        public IEnumerable<IItem> EquippedItems { get { return _equippedItems; } }
        public IEnumerable<IItem> Inventory { get { return _inventory; } }
        public IEnumerable<IAbility> Abilities { get { return _abilities; } }
        public int Hp { get; private set; }
        public int Gold { get; private set; }
        public Player() 
        {
            _equippedItems = new List<IItem>();
            _inventory = new List<IItem>();
            _abilities = new List<IAbility>();
            Hp = 200;
            Gold = 100;
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
        public void EquipItem (IItem item)
        {
            _inventory.Remove(item);
            _equippedItems.Add(item);
        }
        public void UnEquipItem (IItem item)
        {
            _equippedItems.Remove(item);
            _inventory.Add(item);
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
        public void GiveGold(int input)
        {
            Gold -= input;
        }
        public void ReceiveGold(int input)
        {
            Gold += input;
        }
    }
}
