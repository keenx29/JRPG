using JRPG.Abstract;
using JRPG.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public abstract class Quest : IQuest
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Experience { get; private set; }
        public Func<Entity, bool> Requirement { get; private set; }
        public IItem Reward { get; private set; }

        public Quest(string title, string description, int experience, Func<Entity, bool> requirement, IItem reward)
        {
            Title = title;
            Description = description;
            Experience = experience;
            Requirement = requirement;
            Reward = reward;
        }
        public virtual void CompleteQuest(Entity entity)
        { 
            var player = entity.GetComponent<PlayerComponent>().Player;
            if (Requirement(entity) == true)
            {
                player.AddItem(Reward);
            }
        }
    }
}
