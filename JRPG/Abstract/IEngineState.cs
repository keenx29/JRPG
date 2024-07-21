using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IEngineState : IDisposable
    {
        void ProcessInput(ConsoleKeyInfo key);
        void Activate();
        void Deactivate();
    }
}
