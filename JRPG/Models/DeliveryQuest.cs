using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class DeliveryQuest : Quest
    {
        public DeliveryQuest(string title, string description, int experience, IItem reward, QuestState state = QuestState.Completed) : base(title, description, experience, reward, state)
        {

        }

        protected override void QuestActive()
        {
            
        }

        protected override void QuestCompleted()
        {
            
        }
    }
}
