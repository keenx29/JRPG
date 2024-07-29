using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Weapon : Item, IEquippableItem
    {
        public Weapon(string name, int? attack = null, int? weight = null) : base(name)
        {
            Attack = attack ?? 5;
            Weight = weight ?? 5;
        }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Weight { get; private set; }

        public int GetDamageModifier()
        {
            return Attack;
        }

        public int GetDefenseModifier()
        {
            return Defense;
        }

        public Damage ModifyDamage(Damage damage)
        {
            return damage.IncreaseAmount(damage.Amount);
        }
    }
}
