using JRPG.Abstract;
using JRPG.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public abstract class Quest : IQuest
    {
        protected QuestChannel _questsChannel;
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Experience { get; private set; }
        public IItem Reward { get; private set; }
        public QuestState State { get; private set; }
        public int LevelRequirement { get; } //TODO: Level requirement on quests

        public Quest(string title, string description, int experience, IItem reward, QuestState state,QuestChannel questChannel)
        {
            Title = title;
            Description = description;
            Experience = experience;
            Reward = reward;
            State = state;
            _questsChannel = questChannel;
        }
        public void ActivateQuest()
        {
            if (State == QuestState.Pending)
            {
                State = QuestState.Active;
                Enable();
            }
        }
        public void DeactivateQuest()
        {
            if (State == QuestState.Active)
            {
                State = QuestState.Pending;
                Disable();
            }
        }
        protected virtual void Enable()
        {
            _questsChannel.QuestActivatedEvent += QuestActiveEvent;
            _questsChannel.QuestCompleteEvent += QuestCompletedEvent;

            if (State == QuestState.Active)
            {
                QuestActive();
            }
        }

        protected virtual void Disable()
        {
            _questsChannel.QuestActivatedEvent -= QuestActiveEvent;
            _questsChannel.QuestCompleteEvent -= QuestCompletedEvent;
        }
        private void QuestActiveEvent (IQuest activeQuest)
        {
            if (activeQuest.Title != Title)
            {
                return;
            }
            State = QuestState.Active;
            QuestActive();
        }

        protected abstract void QuestActive();

        private void QuestCompletedEvent(IQuest completedQuest)
        {
            if (completedQuest.Title != Title) return;

            State = QuestState.Completed;
            QuestCompleted();
        }

        protected abstract void QuestCompleted();

        protected void Complete()
        {
            State = QuestState.Completed;
            _questsChannel.CompleteQuest(this);
        }
        //public virtual void CompleteQuest(Entity entity)
        //{ 
        //    var player = entity.GetComponent<PlayerComponent>().Player;
        //    if (Requirement(entity) == true)
        //    {
        //        player.AddItem(Reward);
        //    }
        //}
    }
}
