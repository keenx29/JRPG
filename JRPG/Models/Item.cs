using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Item : IItem
    {
        private int _damageModifier;
        private int _totalDamage;
        public string Name {  get; private set; }
        public Item(string name, int? damageModifier = null, int? totalDamage = null) 
        {
            Name = name;
            _damageModifier = damageModifier ?? 0;
            _totalDamage = totalDamage ?? 0;
        }

        public Damage GetDamage(ICombatEntity entity)
        {
            return new Damage(Name, _totalDamage);
        }

        public Damage ModifyDamage(Damage damage)
        {
            return damage.ModifyAmount(_damageModifier);
        }
        public int GetDamageModifier()
        {
            return _damageModifier;
        }
    }
}
