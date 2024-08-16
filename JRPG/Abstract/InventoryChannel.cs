using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public class InventoryChannel
    {
        public Action<IItem> ItemAddedEvent;
        public Action<IItem> ItemRemovedEvent;
        public Action<IEquippableItem> ItemEquippedEvent;
        public Action<IEquippableItem> ItemUnequippedEvent;
        public void AddItem(IItem item)
        {
            ItemAddedEvent?.Invoke(item);
        }
        public void RemoveItem(IItem item)
        {
            ItemRemovedEvent?.Invoke(item);
        }
        public void EquipItem(IEquippableItem item)
        {
            ItemEquippedEvent?.Invoke(item);
        }
        public void UnequipItem(IEquippableItem item)
        {
            ItemUnequippedEvent?.Invoke(item);
        }
    }
}
