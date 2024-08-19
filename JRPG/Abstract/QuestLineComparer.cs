using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JRPG.Abstract
{
    public class QuestLineComparer : IEqualityComparer<IQuestLine>
    {
        public bool Equals([AllowNull] IQuestLine x, [AllowNull] IQuestLine y)
        {
            if (x.Name == y.Name)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode([DisallowNull] IQuestLine obj)
        {
            throw new NotImplementedException();
        }
    }
}
