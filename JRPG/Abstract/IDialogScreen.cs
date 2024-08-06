using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IDialogScreen
    {
        bool IsFinalScreen { get; }
        string Text { get; }
        string Title { get; }
        //bool FinalScreen { get; }
        //string Text { get; }
        Dictionary<string,IDialogScreen> OptionalScreens { get; }
        void EnterScreen(Entity entity);
    }
}
