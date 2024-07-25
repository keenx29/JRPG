using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IItem
    {
        string Name { get; }
        //TODO: The calculation for GetDamage for IUsableItem(s) has been moved to the Consumable class. ModifyDamage calculation should be moved to the IEquippableItem interface and Gear class
    }
}
