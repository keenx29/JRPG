using JRPG.Models;
using System.Collections.Generic;

namespace JRPG.Abstract
{
    public interface IQuestLine
    {
        string Name { get; }
        IEnumerable<IQuest> Quests { get; }
        IEnumerable<IQuest> RemainingQuests { get; }
        bool IsCompleted { get; }
        void AddQuest(IQuest quest);
        void Start();
        void CompleteQuest(IQuest quest);
        bool Contains(IQuest quest);
        void Abandon(IQuest quest);
        //Queue<IQuest> Quests { get; }
        //void AddQuest(IQuest quest);
        //void RemoveQuest();
        //bool IsCompleted(IQuest quest);
        //void RemoveQuest(IQuest quest);
    }
}