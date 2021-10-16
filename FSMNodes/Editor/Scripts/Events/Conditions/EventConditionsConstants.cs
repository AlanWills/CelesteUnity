using Celeste.FSM.Nodes.Events.Conditions;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CelesteEditor.FSMNodes.Events.Conditions
{
    public static class EventConditionsConstants
    {
        #region Properties and Fields

        public static readonly List<Type> EventConditionOptions = new List<Type>();
        public static List<string> EventConditionDisplayNames = new List<string>();

        #endregion

        static EventConditionsConstants()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            EventConditionOptions.Clear();
            EventConditionDisplayNames.Clear();

            Type condition = typeof(EventCondition);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (condition.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        Debug.LogFormat("Found EventCondition type: {0}", t.Name);
                        EventConditionOptions.Add(t);
                        EventConditionDisplayNames.Add(t.GetDisplayName());
                    }
                }
            }

            stopWatch.Stop();
            Debug.LogFormat("Loaded {0} Event Listener Node Conditions in {1} seconds", EventConditionOptions.Count, stopWatch.ElapsedMilliseconds / 1000.0f);
        }
    }
}
