using JRPG.Abstract;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    class BarterComponent : Component, IEntityEntranceComponent
    {
        private Barter Barter { get; set; }
        public BarterComponent(Barter barter)
        {
            Barter = barter;
        }
        public bool CanEnter(Entity entity)
        {
            return true;
        }

        public void Enter(Entity entity)
        {
            Program.Engine.PushState(new BarterState(Barter));
        }
    }
}
