﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface ICombatListener
    {
        void DisplayMessage (string message);
        void EndCombat();
        void PlayerDied();
    }
}
