using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JRPG.Models
{
    public class Entity
    {
        private readonly List<IComponent> _components;
        
        public Vector3 Position { get; set; }
        
        public Entity()
        {
            _components = new List<IComponent>();
            Position = new Vector3(0, 0, 0);
        }
        public void AddComponent(IComponent component)
        {
            component.Parent = this;
            _components.Add(component);
        }
        public TComponent GetComponent<TComponent>() where TComponent : class, IComponent
        {
            return _components.OfType<TComponent>().FirstOrDefault();
        }
        
        public IEnumerable<TComponent> GetComponents<TComponent>() where TComponent : class, IComponent
        {
            return _components.OfType<TComponent>().ToList();
        }
        
        public void RemoveComponent(IComponent component)
        {
            if (component.Parent != this)
            {
                throw new ArgumentException();
            }
            _components.Remove(component);
        }
    }
}
