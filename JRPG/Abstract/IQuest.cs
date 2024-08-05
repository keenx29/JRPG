using JRPG.Models;
using System;

namespace JRPG.Abstract
{
    public interface IQuest
    {
        string Title { get; }
        string Description { get; }
        int Experience { get; }
        IItem Requirement { get; }
        IItem Reward { get; }
        QuestState State { get; }
        int LevelRequirement { get; }
        void ActivateQuest();
        void DeactivateQuest();
    }
}