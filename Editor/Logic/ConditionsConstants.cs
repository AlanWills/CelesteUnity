using Celeste.Logic;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CelesteEditor.Logic
{
    public static class ConditionsConstants
    {
        #region Properties and Fields

        public static readonly List<Type> ConditionOptions = new List<Type>();
        public static readonly List<string> ConditionDisplayNames = new List<string>();

        #endregion

        static ConditionsConstants()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            ConditionOptions.Clear();
            ConditionDisplayNames.Clear();

            Type condition = typeof(Condition);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (condition.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        Debug.LogFormat("Found Condition type: {0}", t.Name);
                        ConditionOptions.Add(t);
                        ConditionDisplayNames.Add(t.GetDisplayName());
                    }
                }
            }

            stopWatch.Stop();
            Debug.LogFormat("Loaded {0} If Node Conditions in {1} seconds", ConditionOptions.Count, stopWatch.ElapsedMilliseconds / 1000.0f);
        }
    }
}
