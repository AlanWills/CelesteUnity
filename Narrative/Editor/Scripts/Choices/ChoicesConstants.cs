using Celeste.Narrative;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Choices
{
    [InitializeOnLoad]
    public static class ChoicesConstants
    {
        #region Properties and Fields

        public static readonly List<Type> ChoiceOptions = new List<Type>();
        public static readonly List<string> ChoiceDisplayNames = new List<string>();

        #endregion

        static ChoicesConstants()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            ChoiceOptions.Clear();
            ChoiceDisplayNames.Clear();

            Type choice = typeof(IChoice);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (choice.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    {
                        Debug.Log($"Found Choice type: {t.Name}");
                        ChoiceOptions.Add(t);
                        ChoiceDisplayNames.Add(t.GetDisplayName());
                    }
                }
            }

            stopWatch.Stop();
            Debug.Log($"Loaded {ChoiceOptions.Count} Choices in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");
        }
    }
}
