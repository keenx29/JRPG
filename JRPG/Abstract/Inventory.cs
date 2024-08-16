using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.Abstract
{
    public class Inventory
    {
        private InventoryChannel _inventoryChannel;
        private readonly List<IItem> _inventory;
        private readonly List<IEquippableItem> _equippedItems;
        public IEnumerable<IItem> NonEquippedItems { get { return _inventory; } }
        public IEnumerable<IEquippableItem> EquippedItems { get { return _equippedItems; } }
        public Inventory(InventoryChannel inventoryChannel)
        {
            _inventory = new List<IItem>();
            _equippedItems = new List<IEquippableItem>();
            _inventoryChannel = inventoryChannel;
            this.Enable();
        }
        public void Enable()
        {
            _inventoryChannel.ItemAddedEvent += ItemAdded;
            _inventoryChannel.ItemRemovedEvent += ItemRemoved;
            _inventoryChannel.ItemEquippedEvent += ItemEquipped;
            _inventoryChannel.ItemUnequippedEvent += ItemUnequipped;
        }
        public void Disable()
        {
            _inventoryChannel.ItemAddedEvent -= ItemAdded;
            _inventoryChannel.ItemRemovedEvent -= ItemRemoved;
            _inventoryChannel.ItemEquippedEvent -= ItemEquipped;
            _inventoryChannel.ItemUnequippedEvent -= ItemUnequipped;
        }

        private void ItemEquipped(IEquippableItem item)
        {
            if (_inventory.Contains(item))
            {
                if (!_equippedItems.Any(x => string.Equals(x.Name, item.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    _inventory.Remove(item);
                    _equippedItems.Add(item);
                    //TODO: Subscribe to the same event in PlayerStats to update the stats
                }
            }
        }
        public void ItemUnequipped(IEquippableItem item)
        {
            if (_equippedItems.Any(x => string.Equals(x.Name, item.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                _equippedItems.Remove(item);
                _inventory.Add(item);
                //TODO: Subscribe to the same event in PlayerStats to update the stats
            }
        }

        private void ItemAdded(IItem item)
        {
            _inventory.Add(item);
        }
        private void ItemRemoved(IItem item)
        {
            _inventory.Remove(item);
        }
    }
}
