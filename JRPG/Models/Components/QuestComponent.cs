using JRPG.Abstract;
using JRPG.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models.Components
{
    public class QuestComponent : Component, IEntityEntranceComponent
    {
        private readonly IQuestDialog _dialog;
        private readonly QuestChannel _questChannel;
        public QuestComponent(IQuestDialog dialog,QuestChannel questChannel)
        {
            _dialog = dialog;
            _questChannel = questChannel;
        }
        public bool CanEnter(Entity entity)
        {
            return true;
        }

        public void Enter(Entity entity)
        {
            Program.Engine.PushState(new QuestDialogState(_dialog, entity,Parent, _questChannel));
        }
    }
}
