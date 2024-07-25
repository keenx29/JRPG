using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Weapon : Item, IEquippableItem
    {
        public Weapon(string name) : base(name)
        {
        }

        public int Weight => throw new NotImplementedException();

        public int Defense => throw new NotImplementedException();
        public int Attack => throw new NotImplementedException();

        public int GetDamageModifier()
        {
            throw new NotImplementedException();
        }

        public Damage ModifyDamage(Damage damage)
        {
            throw new NotImplementedException();
        }
    }
}
