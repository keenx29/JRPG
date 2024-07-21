using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public abstract class Component : IComponent
    {
        public Entity Parent { get; set; }
    }
}
