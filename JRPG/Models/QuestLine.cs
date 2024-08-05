using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using JRPG.Abstract;

namespace JRPG.Models
{
    public class QuestLine : IQuestLine
    {
        public Queue<IQuest> Quests { get; }

        public string Name { get; private set; }

        public QuestLine(string name)
        {
            Quests = new Queue<IQuest>();
            Name = name;
        }
        public void AddQuest(IQuest quest)
        {
            Quests.Enqueue(quest);
        }
        public void CompleteQuest()
        {
            Quests.Dequeue();
        }
    }
}
