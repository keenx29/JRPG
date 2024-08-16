using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public class PlayerStats
    {
        private readonly QuestChannel _questChannel;
        private readonly InventoryChannel _inventoryChannel;
        private int Experience;
        public int Hp { get; set; }
        public int AttackSpeed { get; set; }
        public int AttackDamage { get; set; }
        public int Defense { get; set; }
        public int DamageBuff { get; set; }
        public Action<int> PlayerReceivedExperience;

        public PlayerStats(int hp,QuestChannel questChannel,InventoryChannel inventoryChannel, int? attackSpeed = null, int? attackDamage = null, int? defense = null, int? damageBuff = null, int? experience = null)
        {
            Hp = hp;
            AttackSpeed = attackSpeed ?? 0;
            AttackDamage = attackDamage ?? 0;
            Defense = defense ?? 0;
            DamageBuff = damageBuff ?? 0;
            Experience = experience ?? 0;
            _questChannel = questChannel;
            _inventoryChannel = inventoryChannel;
            this.Enable();
        }
        public void Enable()
        {
            _questChannel.QuestCompleteEvent += QuestCompleteEvent;
            _inventoryChannel.ItemEquippedEvent += ItemEquippedEvent;
            _inventoryChannel.ItemUnequippedEvent += ItemUnequippedEvent;
        }

        private void ItemUnequippedEvent(IEquippableItem item)
        {
            AttackSpeed += item.Weight;
            AttackDamage -= item.Attack;
            Defense -= item.Defense;
        }

        private void ItemEquippedEvent(IEquippableItem item)
        {
            AttackSpeed -= item.Weight;
            AttackDamage += item.Attack;
            Defense += item.Defense;
        }

        public void Disable()
        {
            _questChannel.QuestCompleteEvent -= QuestCompleteEvent;
        }
        private void QuestCompleteEvent(IQuest completedQuest)
        {
            Experience += completedQuest.Experience;
            PlayerReceivedExperience?.Invoke(Experience);
        }
        public void TakeDamage(Damage damage)
        {
            Hp -= damage.Amount;
            // damage = _equippedItems.Aggregate(damage, (a, i) => i.ModifyDamage(a));
        }
        
        public void Heal(int amount)
        {
            Hp += amount;
        }
        public void Buff(int amount)
        {
            DamageBuff += amount;
        }
        public void ResetBuff()
        {
            DamageBuff = 0;
        }
        
    }
}
