using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Gear : Item, IEquippableItem
    {
        public int Weight { get; private set; }
        public Gear(string name, bool canEquip, bool canUse, int? damageModifier = null, int? totalDamage = null,int? weight = null) : base(name, canEquip, canUse, damageModifier, totalDamage)
        {
            Weight = weight ?? 5;
        }
    }
}
