using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Quest : IQuest
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Experience { get; private set; }
        public Action<Entity> Requirement { get; private set; }
        public IItem Reward { get; private set; }

        public Quest(string title, string description, int experience, Action<Entity> requirement, IItem reward)
        {
            Title = title;
            Description = description;
            Experience = experience;
            Requirement = requirement;
            Reward = reward;
        }
    }
}
