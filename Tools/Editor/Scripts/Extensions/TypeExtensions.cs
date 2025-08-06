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
            List<Type> _types = new List<Type>();
            List<string> _displayNames = new List<string>();
            
            LoadTypes<T>(_types, _displayNames);
            
            types = _types.ToArray();
            displayNames = _displayNames.ToArray();
        }
        
        public static void LoadTypes<T>(List<Type> types, List<string> displayNames)
        {
            types.Clear();
            displayNames.Clear();
            
            var namesAndTypes = LoadTypes<T>();
            types.Capacity = namesAndTypes.Count;
            displayNames.Capacity = displayNames.Count;

            foreach (var nameAndType in namesAndTypes)
            {
                displayNames.Add(nameAndType.Item1);
                types.Add(nameAndType.Item2);
            }
        }

        private static List<ValueTuple<string, Type>> LoadTypes<T>()
        {
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            Type targetType = typeof(T);
            List<ValueTuple<string, Type>> namesAndTypes = new List<ValueTuple<string, Type>>();
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (targetType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t.GetCustomAttribute<ObsoleteAttribute>() == null)
                    {
                        Debug.Log($"Found {targetType.Name} type: {t.Name}");
                        namesAndTypes.Add(new ValueTuple<string, Type>(t.GetDisplayName(), t));
                    }
                }
            }
            
            namesAndTypes.Sort((x, y) => string.CompareOrdinal(x.Item1, y.Item1));

            stopWatch.Stop();
            Debug.Log($"Loaded {namesAndTypes.Count} {targetType.Name}s in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");

            return namesAndTypes;
        }
    }
}
