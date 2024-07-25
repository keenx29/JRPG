using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IEquippableItem : IItem
    {
        int Weight { get; }
        int Defense { get; }
        int Attack { get; }
        Damage ModifyDamage(Damage damage);
        int GetDamageModifier();
    }
}
