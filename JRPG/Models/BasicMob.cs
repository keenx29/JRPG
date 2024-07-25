using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class BasicMob : ICombatEntity
    {
        public string Name { get { return "Basic Mob"; } }
        public int Hp { get; private set; }

        public int AttackSpeed {  get; private set; }
        public int Defense {  get; private set; }

        public BasicMob()
        {
            Hp = 100;
            AttackSpeed = 10;
            Defense = 10;
        }
       
        public Damage GetDamage(Player player)
        {
            return new Damage("Kick!", 10);
        }

        public void TakeDamage(Damage damage)
        {
            Hp -= damage.Amount;
        }
    }
}
