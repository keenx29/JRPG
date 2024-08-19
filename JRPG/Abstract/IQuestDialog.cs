using System.Collections.Generic;

namespace JRPG.Abstract
{
    public interface IQuestDialog
    {
        List<IQuestLine> QuestLines { get; }
    }
}