using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IAbility
    {
        string Name { get; }
        Damage CalculateDamage(ICombatEntity entity);
        Damage GetDamage();
    }
}
