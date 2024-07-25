using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Consumable : Item, IUsableItem
    {
        public int Charges { get; private set; }
        public Consumable(string name,int? charges = null, int? damageModifier = null, int? totalDamage = null) : base(name, false, true, damageModifier, totalDamage)
        {
            Charges = charges ?? 1;
        }

        public void UseCharge()
        {
            Charges--;
        }
    }
}
