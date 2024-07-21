using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.Models
{
    public class Barter
    {
        private readonly List<IBarterListener> _listeners;
        public Player Player { get; private set; }
        public Trader Entity { get; private set; }

        public Barter(Player player, Trader entity)
        {
            Player = player;
            Entity = entity;
            var itemList = Player.Inventory.Concat(entity.Inventory).ToList();
            Entity.GeneratePrices(itemList);
        }
        public void AddListener(IBarterListener listener)
        {
            _listeners.Add(listener);
        }
        public void RemoveListener(IBarterListener listener)
        {
            _listeners.Remove(listener);
        }
        public void BuyItem(IItem item)
        {
            var price = Entity.GetPrice(item);
            if (Entity.Inventory.Contains(item) && price <= Player.Gold)
            {
                Player.GiveGold(price);
                Entity.RemoveItem(item);
                Player.AddItem(item);
            }
        }
        public void SellItem(IItem item)
        {
            var price = Entity.GetPrice(item);
            Entity.AddItem(item);
            Player.ReceiveGold(price);
            Player.RemoveItem(item);
        }
    }
}
