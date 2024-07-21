using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IDialogScreen
    {
        bool FinalScreen { get; }
        string Text { get; }
        Dictionary<string,IDialogScreen> NextScreens { get; }
        void EnterScreen(Entity entity);
    }
}
