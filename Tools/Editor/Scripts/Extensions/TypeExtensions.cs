using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CelesteEditor.Tools.Utils
{
    public static class TypeExtensions
    {
        public static void LoadTypes<T>(ref Type[] types, ref string[] displayNames)
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            Type targetType = typeof(T);
            List<Type> _types = new List<Type>();
            List<string> _displayNames = new List<string>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (targetType.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        Debug.Log($"Found {targetType.Name} type: {t.Name}");
                        _types.Add(t);
                        _displayNames.Add(t.GetDisplayName());
                    }
                }
            }

            types = _types.ToArray();
            displayNames = _displayNames.ToArray();

            stopWatch.Stop();
            Debug.Log($"Loaded {types.Length} {targetType.Name}s in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");
        }
    }
}
