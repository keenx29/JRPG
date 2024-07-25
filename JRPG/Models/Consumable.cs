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
        public int DamageBuff { get; private set; }

        public Consumable(string name,int? charges = null,int? damage = null, int? armor = null, int? health = null,int? damageBuff = null) : base(name)
        {

            Charges = charges ?? 1;
            Damage = damage ?? 0;
            Armor = armor ?? 0;
            Health = health ?? 0;
            DamageBuff = damageBuff ?? 0;
        }

        public void UseCharge()
        {
            Charges--;
        }
        public string GetEffect()
        {
            if (Damage != 0)
            {
                return "Damage";
            }
            else if (Armor != 0)
            {
                return "Defense";
            }
            else if (Health != 0)
            {
                return "Health";
            }
            else if (DamageBuff != 0)
            {
                return "Damage Bonus";
            }
            else
            {
                return "nothing";
            }
        }
        public int GetAmount()
        {
            if (Damage > 0)
            {
                return Damage;
            }if (Armor > 0)
            {
                return Armor;
            }if (Health > 0)
            {
                return Health;
            }if (DamageBuff > 0)
            {
                return DamageBuff;
            }
            return 0;
        }
        public Damage GetDamage()
        {
            return new Damage(Name, Damage);
        }
        public Damage CalculateDamage(Player player,ICombatEntity entity)
        {
            if (Damage > 0)
            {
                var updatedDamage = Damage - entity.Defense + player.DamageBuff;
                return new Damage(Name, updatedDamage);
            }
            return new Damage(Name, 0);
        }
    }
}
