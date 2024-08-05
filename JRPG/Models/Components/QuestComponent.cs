using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    public class QuestComponent : Component, IEntityEntranceComponent
    {
        public bool CanEnter(Entity entity)
        {
            return true;
        }

        public void Enter(Entity entity)
        {
            
        }
    }
}
