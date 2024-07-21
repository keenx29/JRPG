using JRPG.Abstract;
using JRPG.Models;
using JRPG.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.States
{
    public class ZoneRenderer : IZoneListener
    {
        private readonly Zone _zone;
        private readonly SpriteComponent[,,] _spriteBuffer;
        
        public bool IsActive { get; set; }

        public ZoneRenderer(Zone zone)
        {
            IsActive = true;
            _zone = zone;
            _spriteBuffer = new SpriteComponent[zone.Size.X, zone.Size.Y, zone.Size.Z];
            foreach (var entity in _zone.Entities)
            {

                var component = entity.GetComponent<SpriteComponent>();
                if (component == null)
                {
                    continue;
                }
                _spriteBuffer[entity.Position.X, entity.Position.Y, entity.Position.Z] = component;
            }
        }
        public void RenderAll()
        {
            Console.Clear();
            Console.WriteLine("ZONE: {0} ",_zone.Name.ToUpper());
            foreach (var sprite in _spriteBuffer)
            {
                if (sprite == null)
                {
                    continue;
                }
                WriteCharacter(sprite.Parent.Position, sprite.Sprite);
            }
        }
        public void EntityAdded(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            if (sprite == null)
            {
                return;
            }
            _spriteBuffer[entity.Position.X, entity.Position.Y, entity.Position.Z] = sprite;

            if (!IsActive)
            {
                return;
            }

            var topmostEntity = GetTopmostEntity(entity.Position);

            if (topmostEntity != null && topmostEntity.Parent.Position.Z > entity.Position.Z)
            {
                return;
            }

            Console.SetCursorPosition(entity.Position.X, entity.Position.Y);
            Console.Write(sprite.Sprite);
        }

        public void EntityMoved(Entity entity, Vector3 newPosition)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            if (sprite == null)
            {
                return;
            }
            _spriteBuffer[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;

            var lastTopmostEntity = GetTopmostEntity(entity.Position);
            var nextTopmostEntity = GetTopmostEntity(newPosition);

            _spriteBuffer[newPosition.X, newPosition.Y, newPosition.Z] = sprite;

            if (!IsActive)
            {
                return;
            }

            if (lastTopmostEntity != null)
            {
                WriteCharacter(entity.Position, lastTopmostEntity.Sprite);
            }
            else
            {
                WriteCharacter(entity.Position, ' ');
            }
            
            if (nextTopmostEntity == null || nextTopmostEntity.Parent.Position.Z < newPosition.Z)
            {
                WriteCharacter(newPosition, sprite.Sprite);
            }
        }


        public void EntityRemoved(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            if (sprite == null)
            {
                return;
            }

            _spriteBuffer[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;

            if (!IsActive)
            {
                return;
            }
            var topmostEntity = GetTopmostEntity(entity.Position);
            if (topmostEntity == null)
            {
                WriteCharacter(entity.Position, ' ');
                return;
            }
            WriteCharacter(topmostEntity.Parent.Position, topmostEntity.Sprite);
        }
        private SpriteComponent GetTopmostEntity(Vector3 position)
        {
            SpriteComponent nextEntity = null;
            for (int i = _zone.Size.Z - 1; i >= 0; i--)
            {
                nextEntity = _spriteBuffer[position.X, position.Y, i];
                if (nextEntity == null)
                {
                    continue;
                }
                return nextEntity;
            }
            return null;
        }
        private void SetCursorPosition (Vector3 position)
        {
            Console.SetCursorPosition(position.X, position.Y + 1);
        }
        private void WriteCharacter(Vector3 position,char character)
        {
            SetCursorPosition(position);
            Console.Write(character);
        }
    }
}
