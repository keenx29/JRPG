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
        public void UseItem(IUsableItem item)
        {
            if (item is IUsableItem && item.Charges > 0)
            {
                var effect = item.GetEffect();
                if (effect == "attack")
                {
                    var damage = item.GetDamage(Entity);
                    PerformAction(damage);
                    item.UseCharge();
                }
                else if (effect == "protect")
                {
                    //TODO: Add armor to the player to reduce the damage
                    //TODO: Make the armor last for a specific turn number
                    item.UseCharge();
                }
                else if (effect == "heal")
                {
                    var healingAmount = item.GetAmount(effect);
                    Player.Heal(healingAmount);
                    Player.TakeDamage(Entity.GetDamage(Player));
                    //TODO: When the player heals he should lose his turn and take damage from the mob
                    /*TODO: Make the necessary checks for taking damage i.e. check health of the player, check if player died after the damage, display message,
                    Best way to implent it is probably by separating the PerformAction method into 2 for Player taking damage and Entity taking damage*/
                    item.UseCharge();
                    
                }
                if (item.Charges == 0)
                {
                    Player.RemoveItem(item);
                }
                return;
            }
            else
            {
                _listeners.ForEach(x => x.DisplayMessage("Can't use " + item.Name));
                return;
            }
            
        }
        public void UseAbility(IAbility ability)
        {
            PerformAction(ability.GetDamage(Entity));
        }
        private void PerformAction (Damage damage)
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            if (Player.AttackSpeed >= Entity.AttackSpeed)
            {
                listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " took " + damage.Amount + " damage from " + damage.Text));
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
            else
            {
                var enemyDamage = Entity.GetDamage(Player);
                listenersCopy.ForEach(x => x.DisplayMessage("Player took " + enemyDamage.Amount + " damage from " + enemyDamage.Text));
                Player.TakeDamage(enemyDamage);
                if (Player.Hp <= 0)
                {
                    listenersCopy.ForEach(x => x.PlayerDied());
                }
                listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " took " + damage.Amount + " damage from " + damage.Text));
                Entity.TakeDamage(damage);

                if (Entity.Hp <= 0)
                {
                    listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " died."));
                    listenersCopy.ForEach(x => x.EndCombat());
                    return;
                }
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
