using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.Models
{
    public class Dialog : IDialog
    {
        //public IDialogScreen FirstScreen { get; private set; }

        public List<IDialogScreen> Screens {  get; private set; }

        public Dialog(IDialogScreen screen = null, List<IDialogScreen> screens = null)
        {
            if (screens == null)
            {
                Screens = new List<IDialogScreen>();
                if (screen != null)
                {
                    Screens.Add(screen);
                }
            }
            else
            {
                Screens = screens;
            }
            
            
        }

        //public void EnterScreen(Entity entity)
        //{
        //    return;
        //}
    }
}
