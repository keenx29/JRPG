﻿using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IBarterEntity
    {
        string Name { get; }
        IEnumerable<IItem> Inventory { get; }
        int GetBuyPrice(IItem item);
        int GetSalePrice (IItem item);
    }
}
