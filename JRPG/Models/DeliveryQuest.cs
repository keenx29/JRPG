using JRPG.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG.Models
{
    public class DeliveryQuest : Quest
    {
        public DeliveryQuest(string title, string description, int experience, Func<Entity, bool> requirement, IItem reward) : base(title, description, experience, requirement, reward)
        {

        }
    }
}
