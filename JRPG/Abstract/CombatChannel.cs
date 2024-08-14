using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public class CombatChannel
    {
        public Action<ICombatEntity> EnemyDiedEvent;
        public Action<ICombatEntity> PlayerDiedEvent;
        public void EnemyKilled(ICombatEntity killedEnemy)
        {
            EnemyDiedEvent?.Invoke(killedEnemy);
        }
    }
}
