using JRPG.Abstract;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    public class CombatComponent : Component, IEntityEntranceComponent
    {
        // Func of Combat to implement a probability of going into combat
        private readonly Func<Combat> _combatFactory;
        public CombatComponent(Func<Combat> combatFactory)
        {
            _combatFactory = combatFactory;
        }

        public bool CanEnter(Entity entity)
        {
            return true;
        }

        public void Enter(Entity entity)
        {
            Program.Engine.PushState(new CombatState(_combatFactory()));
        }
    }
}
