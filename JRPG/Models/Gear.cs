using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Gear : Item, IEquippableItem
    {
        public int Defense { get; private set; }
        public int Weight { get; private set; }
        public int Attack { get; private set; }
        public Gear(string name,int? defense = null,int? weight = null) : base(name)
        {
            Weight = weight ?? 5;
            Defense = defense ?? 5;
        }

        public Damage ModifyDamage(Damage damage)
        {
            return damage.ReduceAmount(Defense);
        }

        public int GetDamageModifier()
        {
            return Defense;
        }
    }
}
