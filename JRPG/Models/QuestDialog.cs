using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class QuestDialog : IQuestDialog
    {
        public List<IQuestLine> QuestLines { get; private set; }

        public QuestDialog(List<IQuestLine> questLines)
        {
            QuestLines = questLines ?? new List<IQuestLine>();
        }
        public QuestDialog(IQuestLine questLine)
        {
            QuestLines = new List<IQuestLine>
            {
                questLine
            };
        }
    }
}
