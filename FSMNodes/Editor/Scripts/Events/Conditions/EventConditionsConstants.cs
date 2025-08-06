using Celeste.FSM.Nodes.Events.Conditions;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CelesteEditor.Tools.Utils;
using UnityEngine;

namespace CelesteEditor.FSMNodes.Events.Conditions
{
    public static class EventConditionsConstants
    {
        #region Properties and Fields

        public static readonly List<Type> EventConditionOptions = new();
        public static List<string> EventConditionDisplayNames = new();

        #endregion

        static EventConditionsConstants()
        {
            TypeExtensions.LoadTypes<EventCondition>(EventConditionOptions, EventConditionDisplayNames);
        }
    }
}
