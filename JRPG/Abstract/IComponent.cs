using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IComponent
    {
        Entity Parent { get; set; }
    }
}
