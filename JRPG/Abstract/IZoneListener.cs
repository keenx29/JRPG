using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IZoneListener
    {
        // Handles changes in the zone's state
        public void EntityMoved(Entity entity, Vector3 newPosition);
        void EntityAdded(Entity entity);
        void EntityRemoved(Entity entity);
    }
}
