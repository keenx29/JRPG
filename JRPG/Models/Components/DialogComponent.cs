using JRPG.Abstract;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    class DialogComponent : Component, IEntityEntranceComponent
    {
        private readonly IDialog _dialog;
        public DialogComponent(IDialog dialog)
        {
            _dialog = dialog;
        }
        public bool CanEnter(Entity entity)
        {
            return true;
        }

        public void Enter(Entity entity)
        {
            Program.Engine.PushState(new DialogState(_dialog, entity));
        }
    }
}
