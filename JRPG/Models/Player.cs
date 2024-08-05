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
        private readonly List<IQuestLine> _activeQuestLines;
        private readonly List<IQuestLine> _completedQuestLines;
        private readonly QuestChannel _questChannel;

        public IEnumerable<IEquippableItem> EquippedItems { get { return _equippedItems; } }
        public IEnumerable<IItem> Inventory { get { return _inventory; } }
        public IEnumerable<IAbility> Abilities { get { return _abilities; } }
        public IEnumerable<IQuestLine> ActiveQuestLines { get { return _activeQuestLines; } }
        public IEnumerable<IQuestLine> CompletedQuestLines { get { return _completedQuestLines; } }
        public PlayerStats Stats { get; private set; }
        public int Gold { get; private set; }
        
        public Player(QuestChannel questChannel) 
        {
            _equippedItems = new List<IEquippableItem>();
            _inventory = new List<IItem>();
            _abilities = new List<IAbility>();
            _activeQuestLines = new List<IQuestLine>();
            _completedQuestLines = new List<IQuestLine>();
            _questChannel = questChannel;
            _questChannel.QuestCompleteEvent += OnQuestCompleted;
            _questChannel.QuestActivatedEvent += OnQuestActivated;
            Gold = 1000;
            Stats = new PlayerStats(200, _questChannel, attackSpeed: 10);
        }
        private void OnQuestCompleted(IQuest quest)
        {
            CompleteQuest(quest);
        }
        private void OnQuestActivated(IQuest quest)
        {
            var questLine = _activeQuestLines.FirstOrDefault(qLine => qLine.Contains(quest));
            if (questLine != null)
            {
                StartQuestLine(questLine);
            }
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
        public void UpdateStats()
        {
            foreach (IEquippableItem item in EquippedItems)
            {
                if (item.Weight > 0)
                {
                    Stats.AttackSpeed -= item.Weight;
                }
                if (item.Defense > 0)
                {
                    Stats.Defense += item.Defense;
                }
                if (item.Attack > 0)
                {
                    Stats.AttackDamage += item.Attack;
                }
            }
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

        
        public void SpendGold(int input)
        {
            Gold -= input;
        }
        public void ReceiveGold(int input)
        {
            Gold += input;
        }
        
        
        public void StartQuestLine(IQuestLine questLine)
        {
            //if (!_quests.Contains(questLine))
            //{
            //    _quests.Add(questLine);
            //}
            if (!_activeQuestLines.Contains(questLine))
            {
                _activeQuestLines.Add(questLine);
            }
            questLine.Start();
        }
        public void CompleteQuest(IQuest quest)
        {
            var questLine = _activeQuestLines.FirstOrDefault(qLine => qLine.Contains(quest));
            if (questLine != null)
            {
                questLine.CompleteQuest(quest);

                if (questLine.IsCompleted)
                {
                    _activeQuestLines.Remove(questLine);
                    _completedQuestLines.Add(questLine);
                }
            }
        }
        public void StartQuestLine(IQuestLine questLine)
        {
            if (!_quests.Contains(questLine))
            {
                _quests.Add(questLine); 
            }
        }
    }
}
