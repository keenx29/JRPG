using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class KillQuest : Quest
    {
        private CombatChannel _combatChannel;
        private InventoryChannel _inventoryChannel;
        public ICombatEntity EnemyToDefeat { get; private set; }
        public int NumberOfEnemiesToDestroy { get; }
        public int EnemiesDefeated { get; private set; }
        public KillQuest(string title, string description,ICombatEntity enemyToDefeat,int numberOfEnemiesToDestroy, int experience, IItem reward,CombatChannel combatChannel,QuestChannel questChannel,InventoryChannel inventoryChannel,QuestState state = QuestState.Pending) : base(title, description, experience, reward, state, questChannel)
        {
            _combatChannel = combatChannel;
            _inventoryChannel = inventoryChannel;
            NumberOfEnemiesToDestroy = numberOfEnemiesToDestroy;
            EnemyToDefeat = enemyToDefeat;
            Enable();
        }
        protected override void Enable()
        {
            base.Enable();
            EnemiesDefeated = 0;
        }
        protected override void QuestActive()
        {
            _combatChannel.EnemyDiedEvent += EnemyDiedEvent;
        }

        protected override void QuestCompleted()
        {
            _combatChannel.EnemyDiedEvent -= EnemyDiedEvent;

        }
        private void EnemyDiedEvent(ICombatEntity defeatedEnemy)
        {
            if (defeatedEnemy.GetType() == EnemyToDefeat.GetType())
            {
                this.EnemiesDefeated++;
            }

            if (EnemiesDefeated == NumberOfEnemiesToDestroy)
            
            {
                this.Complete();
            }
        }

        protected override void QuestDelivered()
        {
            if (Reward != null)
            {
                _inventoryChannel.AddItem(Reward); 
            }
        }
    }
}
