using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class DeliveryQuest : Quest
    {
        private readonly QuestChannel _questChannel;
        private readonly InventoryChannel _inventoryChannel;
        public DeliveryQuest(string title, string description, int experience, IItem reward, QuestChannel questChannel,InventoryChannel inventoryChannel, Entity startPoint, Entity endPoint, QuestState state = QuestState.Pending) : base(title, description, experience, reward, state, startPoint, endPoint, questChannel)
        {
            _questChannel = questChannel;
            _inventoryChannel = inventoryChannel;
        }

        protected override void QuestActive()
        {
            Complete();
        }

        protected override void QuestCompleted()
        {
            
        }

        protected override void QuestDelivered()
        {
            if (Reward != null)
            {
                _inventoryChannel.AddItem(Reward); 
            }
        }
    }
}
