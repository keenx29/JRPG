using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class DialogScreen : IDialogScreen
    {
        private Action<Entity> _reward;
        public bool IsFinalScreen { get; private set; }
        public bool Completed { get; private set; }

        public string Text  { get; private set; }

        public string Title { get; private set; }

        public Dictionary<string, IDialogScreen> OptionalScreens { get; private set; }
        public DialogScreen(string title,string text,
            Action<Entity> reward = null,
            Dictionary<string, IDialogScreen> optionalScreens = null,
            bool isFinalScreen = false)
        {
            Title = title;
            Text = text;
            _reward = reward;
            OptionalScreens = optionalScreens ?? new Dictionary<string, IDialogScreen>();
            IsFinalScreen = isFinalScreen;
        }
        public void EnterScreen(Entity entity)
        {
            if (_reward != null)
            {
                _reward(entity);
            }
        }

        //private Action<Entity> _enterAction;

        //public bool FinalScreen { get { return NextScreens.Count == 0; } }
        //public string Text { get; private set; }
        //public Dictionary<string, IDialogScreen> NextScreens { get; private set; }
        //public DialogScreen(string text,
        //    Action<Entity> enterAction = null,
        //    Dictionary<string, IDialogScreen> nextScreens = null)
        //{
        //    Text = text;
        //    _enterAction = enterAction;
        //    NextScreens = nextScreens ?? new Dictionary<string, IDialogScreen>();
        //}
        //public void EnterScreen(Entity entity)
        //{
        //    if (_enterAction != null)
        //    {
        //        _enterAction(entity);
        //    }
        //}
    }
}
