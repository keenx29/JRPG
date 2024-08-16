using JRPG.Abstract;
using JRPG.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JRPG.Models
{
    public class Combat
    {
        private readonly CombatChannel _combatChannel;
        private readonly InventoryChannel _inventoryChannel;
        private readonly List<ICombatListener> _listeners;
        public Player Player { get; private set; }
        public ICombatEntity Entity { get; private set; }
        public Combat(Player player, ICombatEntity entity, CombatChannel combatChannel,InventoryChannel inventoryChannel)
        {
            _listeners = new List<ICombatListener>();
            _combatChannel = combatChannel;
            _inventoryChannel = inventoryChannel;
            Player = player;
            Entity = entity;
        }
        public void UseItem(IUsableItem item)
        {
            if (item.Charges > 0)
            {
                var effect = item.GetEffect();
                var modifier = item.GetAmount();
                switch (effect)
                {
                    case "Damage":
                        var damage = item.GetDamage();
                        PerformAction(damage);
                        break;
                    case "Protect":
                        //TODO: Add armor to the player to reduce the damage
                        //TODO: Make the armor last for a specific turn number
                        break;
                    case "Heal":
                        HealPlayer(item);
                        break;
                    case "Damage Bonus":
                        BuffPlayer(item);
                        break;
                    default:
                        break;
                }
                //if (effect == "attack")
                //{
                //    var damage = item.GetDamage(Player,Entity);
                //    var updatedDamage = new Damage(damage.Text, damage.Amount + Player.DamageBuff);
                //    PerformAction(updatedDamage);
                //}
                //else if (effect == "protect")
                //{
                //    
                //}
                //else if (effect == "heal")
                //{
                //    HealPlayer(item);
                //    
                //}
                //else if (effect == "buff")
                //{
                //    BuffPlayer(item);
                //}
                item.UseCharge();
                if (item.Charges == 0)
                {
                    _inventoryChannel.RemoveItem(item);
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
            var rawAbilityDamage = ability.GetDamage();
            var calculatedAbilityDamage = new Damage
                (rawAbilityDamage.Text,
                rawAbilityDamage.Amount + (Player.Stats.AttackDamage / 2) + Player.Stats.DamageBuff - (Entity.Defense / 2));
            PerformAction(calculatedAbilityDamage);
        }
        private void HealPlayer(IUsableItem item)
        {
            var healingAmount = item.GetAmount();
            var listenersCopy = new List<ICombatListener>(_listeners);
            listenersCopy.ForEach(x => x.DisplayMessage("Player healed for " + healingAmount + " hp from " + item.Name));
            Player.Stats.Heal(healingAmount);
            DamagePlayer();
        }
        private void BuffPlayer(IUsableItem item)
        {
            var buffAmount = item.GetAmount();
            var listenersCopy = new List<ICombatListener>(_listeners);
            listenersCopy.ForEach(x => x.DisplayMessage("Player buffed for " + buffAmount + " damage from " + item.Name));
            Player.Stats.Buff(buffAmount);
            DamagePlayer();
        }
        private void PerformActionOnPlayer(Damage damage)
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            damage = Entity.GetDamage(Player);
            listenersCopy.ForEach(x => x.DisplayMessage("Player took " + damage.Amount + " damage from " + damage.Text));
            Player.Stats.TakeDamage(damage);
            if (Player.Stats.Hp <= 0)
            {
                listenersCopy.ForEach(x => x.PlayerDied());
            }
        }
        private void PerformAction (Damage damage)
        {
            if (Player.Stats.AttackSpeed >= Entity.AttackSpeed)
            {
                DamageEnemy(damage);
                DamagePlayer();
            }
            else
            {
                DamagePlayer();
                DamageEnemy(damage);
            }
            
        }
        private void DamageEnemy(Damage damage)
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " took " + damage.Amount + " damage from " + damage.Text));
            Entity.TakeDamage(damage);

            if (Entity.Hp <= 0)
            {
                
                listenersCopy.ForEach(x => x.DisplayMessage(Entity.Name + " died."));
                _combatChannel.EnemyKilled(Entity);
                listenersCopy.ForEach(x => x.EndCombat());
                return;
            }
        }
        private void DamagePlayer()
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            var rawEnemyDamage = Entity.GetDamage(Player);
            var calculatedEnemyDamage = new Damage(rawEnemyDamage.Text, rawEnemyDamage.Amount - (Player.Stats.Defense / 2));
            listenersCopy.ForEach(x => x.DisplayMessage("Player took " + calculatedEnemyDamage.Amount + " damage from " + calculatedEnemyDamage.Text));
            Player.Stats.TakeDamage(calculatedEnemyDamage);
            if (Player.Stats.Hp <= 0)
            {
                _combatChannel.PlayerDiedEvent?.Invoke(Entity);
                listenersCopy.ForEach(x => x.PlayerDied());
            }
        }
        public bool Run()
        {
            var listenersCopy = new List<ICombatListener>(_listeners);
            listenersCopy.ForEach(x => x.EndCombat());
            return true;
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
