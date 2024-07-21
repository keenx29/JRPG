using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class Dialog : IDialog
    {
        public IDialogScreen FirstScreen { get; private set; }

        public Dialog(IDialogScreen firstScreen)
        {
            FirstScreen = firstScreen;
        }

        public void EnterScreen(Entity entity)
        {
            return;
        }
    }
}
