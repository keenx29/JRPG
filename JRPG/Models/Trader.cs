﻿using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Trader : IBarterEntity
    {
        public string Name { get { return "Basic Trader"; } }
        private readonly Dictionary<IItem, int> _prices;
        private readonly List<IItem> _inventory;
        public IEnumerable<IItem> Inventory { get { return _inventory; } }
        public Dictionary<IItem, int> Prices {  get; set; }
        public Trader()
        {
            _inventory = new List<IItem>();
            _prices = new Dictionary<IItem, int>();
        }
        public void GeneratePrices(List<IItem> items)
        {
            var random = new Random();

            foreach (var item in items)
            {
                _prices.Add(item, random.Next(10, 50));
            }

        }
        public int GetPrice(IItem item)
        {
            _prices.TryGetValue(item, out var price);
            return price;
        }
        public void AddItem(IItem item)
        {
            _inventory.Add(item);
        }
        public void RemoveItem(IItem item)
        {
            _inventory.Remove(item);
        }
    }
}
