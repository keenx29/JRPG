using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Consumable : Item, IUsableItem
    {
        public int Charges { get; private set; }

        public int Damage { get; private set; }

        public int Armor {  get; private set; }

        public int Health { get; private set; }

        public Consumable(string name,int? charges = null,int? damage = null, int? armor = null, int? health = null, int? damageModifier = null, int? totalDamage = null) : base(name, damageModifier, totalDamage)
        {

            Charges = charges ?? 1;
            Damage = damage ?? 0;
            Armor = armor ?? 0;
            Health = health ?? 0;
        }

        public void UseCharge()
        {
            Charges--;
        }
        public string GetEffect()
        {
            if (Damage != 0)
            {
                return "attack";
            }
            else if (Armor != 0)
            {
                return "protect";
            }
            else if (Health != 0)
            {
                return "heal";
            }
            else
            {
                return "common";
            }
        }
        public int GetAmount(string effect)
        {
            if (effect == "attack")
            {
                return Damage;
            }
            else if (effect == "protect")
            {
                return Armor;
            }
            else if (effect == "heal")
            {
                return Health;
            }
            else
            {
                return 0;
            }
        }
    }
}
