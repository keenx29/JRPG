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
        
        private readonly List<IAbility> _abilities;
        private readonly List<IQuestLine> _activeQuestLines;
        private readonly List<IQuestLine> _completedQuestLines;
        private readonly QuestChannel _questChannel;

        public Inventory Inventory { get; private set; }
        
        public IEnumerable<IAbility> Abilities { get { return _abilities; } }
        public IEnumerable<IQuestLine> ActiveQuestLines { get { return _activeQuestLines; } }
        public IEnumerable<IQuestLine> CompletedQuestLines { get { return _completedQuestLines; } }
        public PlayerStats Stats { get; private set; }
        public int Gold { get; private set; }
        
        public Player(QuestChannel questChannel,InventoryChannel inventoryChannel) 
        {
            _abilities = new List<IAbility>();
            Inventory = new Inventory(inventoryChannel);
            Inventory.Enable();
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
    }
}
