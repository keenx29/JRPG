using JRPG.Abstract;
using JRPG.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Combat
    {
        private readonly List<ICombatListener> _listeners;
        public Player Player { get; private set; }
        public ICombatEntity Entity { get; private set; }
        public Combat(Player player, ICombatEntity entity)
        {
            _listeners = new List<ICombatListener>();
            Player = player;
            Entity = entity;
        }
        public void UseItem(IItem item)
        {
            if (!item.CanUse)
            {
                _listeners.ForEach(x => x.DisplayMessage("Can't use " + item.Name));
                return;
            }
            PerformAction(item.GetDamage(Entity));
        }
        public void UseAbility(IAbility ability)
        {
            PerformAction(ability.GetDamage(Entity));
        }
        private void PerformAction (Damage damage)
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + "took " + damage.Amount + " damage from " + damage.Text));
            Entity.TakeDamage(damage);

            if (Entity.Hp <= 0)
            {
                listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " died."));
                listenersCopy.ForEach(x => x.EndCombat());
                return;
            }

            damage = Entity.GetDamage(Player);
            listenersCopy.ForEach(x => x.DisplayMessage("Player took " + damage.Amount + " damage from " + damage.Text));
            Player.TakeDamage(damage);
            if (Player.Hp <= 0)
            {
                listenersCopy.ForEach(x => x.PlayerDied());
            }
        }
        public void AddListener(ICombatListener listener)
        {
            _listeners.Add(listener);
        }
        public void RemoveListener(ICombatListener listener)
        {
            _listeners.Remove(listener);
        }

    }
}
