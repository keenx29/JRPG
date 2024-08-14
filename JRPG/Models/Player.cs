using JRPG.Abstract;
using System;
using System.Collections;
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
        private readonly List<IQuestLine> _quests;
        public IEnumerable<IEquippableItem> EquippedItems { get { return _equippedItems; } }
        public IEnumerable<IItem> Inventory { get { return _inventory; } }
        public IEnumerable<IAbility> Abilities { get { return _abilities; } }
        public IEnumerable<IQuestLine> Quests { get { return _quests; } }
        public int Hp { get; private set; }
        public int Gold { get; private set; }
        public int AttackSpeed { get; private set; }
        public int AttackDamage { get; private set; }
        public int Defense { get; private set; }
        public int DamageBuff { get; private set; } 
        public Player() 
        {
            _equippedItems = new List<IEquippableItem>();
            _inventory = new List<IItem>();
            _abilities = new List<IAbility>();
            _quests = new List<IQuestLine>();
            Hp = 200;
            Gold = 1000;
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
                if (item.Defense > 0)
                {
                    Defense += item.Defense;
                }
                if (item.Attack > 0)
                {
                    AttackDamage += item.Attack;
                }
            }
        }
        public void Heal(int amount)
        {
            Hp += amount;
        }
        public void Buff(int amount)
        {
            DamageBuff += amount;
        }
        public void ResetBuff()
        {
            DamageBuff = 0;
        }
        public void StartQuestLine(IQuestLine questLine)
        {
            if (!_quests.Contains(questLine))
            {
                _quests.Add(questLine);
            }
        }
        public void CompleteQuest(IQuestLine questLine)
        {
            foreach (var qLine in _quests)
            {
                if (qLine == questLine)
                {
                    qLine.RemoveQuest();
                }
            }
        }
    }
}
