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
            _activeQuestLines = new List<IQuestLine>();
            _completedQuestLines = new List<IQuestLine>();
            questChannel.QuestDeliveredEvent += OnQuestDelivered;
            questChannel.QuestLineActivatedEvent += OnQuestLineActivated;
            Gold = 1000;
            Stats = new PlayerStats(200, questChannel, inventoryChannel, attackSpeed: 10);
        }
        private void OnQuestDelivered(IQuest quest)
        {
            DeliverQuest(quest);
        }
        private void OnQuestLineActivated(IQuestLine questLine)
        {
            //Adds a QuestLine to the activeQuestLines list if it is not already there
            var isActive = _activeQuestLines.Contains(questLine,new QuestLineComparer());
            if (!isActive)
            {
                _activeQuestLines.Add(questLine);
            }
            //Starts the first quest in the remainingQuests queue
            questLine.Start();
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
        
        public void DeliverQuest(IQuest quest)
        {
            var questLine = _activeQuestLines.FirstOrDefault(qLine => qLine.Contains(quest));
            if (questLine != null)
            {
                questLine.DeliverQuest(quest);

                if (questLine.IsCompleted)
                {
                    _activeQuestLines.Remove(questLine);
                    _completedQuestLines.Add(questLine);
                }
            }
        }
    }
}
