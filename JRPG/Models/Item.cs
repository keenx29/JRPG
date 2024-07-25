using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Item : IItem
    {
        public string Name {  get; private set; }
        public Item(string name) 
        {
            Name = name;
        }

    }
}
