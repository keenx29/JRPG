using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    public class ConstantEntranceComponent : Component,IEntityEntranceComponent
    {
        private readonly bool _canEnter;
        public ConstantEntranceComponent(bool canEnter)
        {
            _canEnter = canEnter;
        }
        public bool CanEnter(Entity entity)
        {
            return _canEnter;
        }

        public void Enter(Entity entity)
        {
            
        }
    }
}
