using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.Abstract
{
    public interface IDialog
    {
        //IDialogScreen FirstScreen { get; }
        List<IDialogScreen> Screens { get; }
    }
}
