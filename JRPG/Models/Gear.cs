using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Gear : Item, IEquippableItem
    {
        public int Weight { get; private set; }
        public Gear(string name, int? damageModifier = null, int? totalDamage = null,int? weight = null) : base(name, damageModifier, totalDamage)
        {
            Weight = weight ?? 5;
        }
    }
}
