using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class KillQuest : Quest
    {
        private CombatChannel _combatChannel;
        private ICombatEntity _enemyToDefeat;
        public int NumberOfEnemiesToDestroy { get; }
        public int EnemiesDefeated { get; private set; }
        public KillQuest(string title, string description,ICombatEntity enemyToDefeat,int numberOfEnemiesToDestroy, int experience, IItem reward,CombatChannel combatChannel,QuestState state = QuestState.Active) : base(title, description, experience, reward, state)
        {
            _combatChannel = combatChannel;
            NumberOfEnemiesToDestroy = numberOfEnemiesToDestroy;
            _enemyToDefeat = enemyToDefeat;
            Enable();
        }
        protected override void Enable()
        {
            base.Enable();
            EnemiesDefeated = 0;
        }
        protected override void QuestActive()
        {
            //TODO: Track an EnemyDiedEvent using a combat channel
            _combatChannel.EnemyDiedEvent += EnemyDiedEvent;
        }

        protected override void QuestCompleted()
        {
            //TODO: Remove tracker from EnemyDiedEvent
            _combatChannel.EnemyDiedEvent -= EnemyDiedEvent;
        }
        private void EnemyDiedEvent(ICombatEntity defeatedEnemy)
        {
            if (defeatedEnemy.GetType() == _enemyToDefeat.GetType())
            {
                this.EnemiesDefeated++;
            }

            if (EnemiesDefeated == NumberOfEnemiesToDestroy)
            {
                this.Complete();
            }
        }
    }
}
