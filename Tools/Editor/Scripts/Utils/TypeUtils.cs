using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CelesteEditor.Tools.Utils
{
    public static class TypeUtils
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
                        Debug.LogFormat("Found {0} type: {1}", targetType.Name, t.Name);
                        _types.Add(t);
                        _displayNames.Add(t.GetDisplayName());
                    }
                }
            }

            types = _types.ToArray();
            displayNames = _displayNames.ToArray();

            stopWatch.Stop();
            Debug.LogFormat("Loaded {0} {1}s in {2} seconds", types.Length, targetType.Name, stopWatch.ElapsedMilliseconds / 1000.0f);
        }
    }
}
