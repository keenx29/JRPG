using JRPG.Models;
using System;

namespace JRPG.Abstract
{
    public interface IQuest
    {
        string Description { get; }
        int Experience { get; }
        Func<Entity,bool> Requirement { get; }
        IItem Reward { get; }
        string Title { get; }
    }
}