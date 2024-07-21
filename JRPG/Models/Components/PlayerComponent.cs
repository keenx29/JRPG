using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    public class PlayerComponent : Component
    {
        public Player Player { get; private set; }
        public PlayerComponent(Player player) 
        {
            Player = player;
        }
    }
}
