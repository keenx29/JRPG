﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public class QuestChannel
    {
        public Action<IQuest> QuestCompleteEvent;
        public Action<IQuest> QuestActivatedEvent;

        public void CompleteQuest (IQuest completedQuest)
        {
            QuestCompleteEvent?.Invoke(completedQuest); 
        }
        public void AssignQuest (IQuest questToAssign)
        {
            QuestActivatedEvent?.Invoke(questToAssign); 
        }
    }
}
