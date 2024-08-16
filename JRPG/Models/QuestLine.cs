using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using JRPG.Abstract;

namespace JRPG.Models
{
    public class QuestLine : IQuestLine
    {
        private Queue<IQuest> _remainingQuests;
        private List<IQuest> _completedQuests;

        public string Name { get; private set; }
        public IEnumerable<IQuest> Quests => _completedQuests.Concat(_remainingQuests);
        public IEnumerable<IQuest> RemainingQuests => _remainingQuests;
        public bool IsCompleted => _remainingQuests.Count == 0;

        public QuestLine(string name)
        {
            Name = name;
            _remainingQuests = new Queue<IQuest>();
            _completedQuests = new List<IQuest>();
        }

        public void AddQuest(IQuest quest)
        {
            _remainingQuests.Enqueue(quest);
        }

        public void Start()
        {
            if (_remainingQuests.Count > 0)
            {
                var firstQuest = _remainingQuests.Peek();
                firstQuest.ActivateQuest();
            }
        }
        public void DeliverQuest(IQuest quest)
        {
            //TODO: Only move the quest to _completedQuests when it has been delivered
            if (_remainingQuests.Count > 0 && _remainingQuests.Peek() == quest && quest.State == QuestState.Delivered)
            {
                _completedQuests.Add(_remainingQuests.Dequeue());
            }
        }
        public void CompleteQuest(IQuest quest)
        {
            //TODO: Only move the quest to _completedQuests when it has been delivered
        }

        public bool Contains(IQuest quest)
        {
            return _remainingQuests.Contains(quest) || _completedQuests.Contains(quest);
        }

        public void Abandon(IQuest quest)
        {
            if (_remainingQuests.Count > 0 && _remainingQuests.Peek() == quest)
        {
                quest.DeactivateQuest();
            }
        }
    }
}
