using JRPG.Models;
using System;

namespace JRPG.Abstract
{
    public interface IQuest
    {
        string Title { get; }
        string Description { get; }
        int Experience { get; }
        IItem Reward { get; }
        QuestState State { get; }
        int LevelRequirement { get; }
        IQuestLine questLine { get; }
        void ActivateQuest();
        void DeactivateQuest();
    }
}