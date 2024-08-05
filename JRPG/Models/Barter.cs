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
            _listeners = new List<IBarterListener>();
            Player = player;
            Entity = entity;
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
            var price = Entity.GetBuyPrice(item);
            if (Entity.Inventory.Contains(item) && price <= Player.Gold)
            {
                Player.SpendGold(price);
                Entity.RemoveItem(item);
                Player.AddItem(item);
            }
        }
        public void SellItem(IItem item)
        {
            var price = (double)Entity.GetSalePrice(item);
            Entity.AddItem(item);
            Player.ReceiveGold((int)price);
            Player.RemoveItem(item);
        }
    }
}
