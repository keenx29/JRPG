using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IUsableItem : IItem
    {
        int Charges { get; }
        void UseCharge();
        string GetEffect();
        int GetAmount();
        Damage CalculateDamage(Player player,ICombatEntity entity);
        Damage GetDamage();
    }
}
