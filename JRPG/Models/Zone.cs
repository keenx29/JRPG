using JRPG.Abstract;
using JRPG.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Zone
    {
        private readonly Entity[,,] _entities;
        private readonly HashSet<IZoneListener> _listeners;
        public Vector3 Size { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Entity> Entities
        {
            get
            {
                for (int x = 0; x < Size.X; x++)
                {
                    for (int y = 0; y < Size.Y; y++)
                    {
                        for (int z = 0; z < Size.Z; z++)
                        {
                            var entity = _entities[x, y, z];
                            if (entity == null)
                            {
                                continue;
                            }
                            yield return entity;
                        }
                    }
                }
            }
        }

        public Zone(string name, Vector3 size)
        {
            _listeners = new HashSet<IZoneListener>();
            Size = size;
            Name = name;
            _entities = new Entity[size.X, size.Y, size.Z];
        }
        public void MoveEntity(Entity entity, Vector3 newPosition)
        {
            if (newPosition.X < 0 || newPosition.X >= Size.X ||
                newPosition.Y < 0 || newPosition.Y >= Size.Y ||
                newPosition.Z < 0 || newPosition.Z >= Size.Z)
            {
                return;
            }

            var topmostEntity = GetTopmostEntity(newPosition);
            if (topmostEntity != null)
            {
                var component = topmostEntity.GetComponent<IEntityEntranceComponent>();
                if (component != null)
                {
                    if (!component.CanEnter(entity))
                    {
                        return;
                    }
                    component.Enter(entity);
                } 
            }

            _listeners.ForEach(l => l.EntityMoved(entity, newPosition));
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;
            entity.Position = newPosition;
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = entity;
        }
        public void AddEntity(Entity entity)
        {
            _listeners.ForEach(x => x.EntityAdded(entity));
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = entity;
        }
        public void RemoveEntity(Entity entity)
        {
            var oldEntity = _entities[entity.Position.X, entity.Position.Y, entity.Position.Z];
            if (oldEntity != entity)
            {
                throw new InvalidOperationException("Entity position is out of sync");
            }
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;
        }
        public void AddListener(IZoneListener listener)
        {
            if (!_listeners.Add(listener))
            {
                throw new ArgumentException();
            }
        }
        public void RemoveListener(IZoneListener listener)
        {
            if (!_listeners.Remove(listener))
            {
                throw new ArgumentException();
            }
        }
        private Entity GetTopmostEntity(Vector3 position)
        {
            for (int i = Size.Z - 1; i >= 0; i--)
            {
                var entity = _entities[position.X, position.Y, i];
                if (entity != null)
                {
                    return entity;
                }

            }
            return null;
        }
    }
}
