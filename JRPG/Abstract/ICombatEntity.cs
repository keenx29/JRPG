using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface ICombatEntity
    {
        string Name { get; }
        int Hp { get; }
        int AttackSpeed { get; }
        Damage GetDamage(Player player);
        void TakeDamage(Damage damage);
    }
}
