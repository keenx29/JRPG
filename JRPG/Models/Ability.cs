using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Ability : IAbility
    {
        private readonly int _damage;
        public string Name {  get; private set; }

        public Ability (string name, int damage)
        {
            Name = name;
            _damage = damage;
        }
        public Damage GetDamage()
        {
            return new Damage(Name, _damage);
        }
        public Damage CalculateDamage(ICombatEntity entity)
        {
            return new Damage(Name, _damage - (entity.Defense / 2));
        }
    }
}
