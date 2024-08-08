using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Abstract
{
    public interface IQuestListener
    {
        void QuestProgressed();
        void QuestCompleted();
    }
}
