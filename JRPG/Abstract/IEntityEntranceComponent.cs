using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IEntityEntranceComponent : IComponent
    {
        bool CanEnter(Entity entity);
        void Enter(Entity entity);
    }
}
