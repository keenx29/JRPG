using JRPG.Models;
using System.Collections.Generic;

namespace JRPG.Abstract
{
    public interface IQuestLine
    {
        string Name { get; }
        Queue<IQuest> Quests { get; }
        void AddQuest(IQuest quest);
        void RemoveQuest();
        //bool IsCompleted(IQuest quest);
        //void RemoveQuest(IQuest quest);
    }
}