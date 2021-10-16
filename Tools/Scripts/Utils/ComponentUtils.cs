using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools
{
    public static class ComponentUtils
    {
        public static void TryGet<T>(this Component monoBehaviour, ref T component) where T : Component
        {
            if (component == null)
            {
                component = monoBehaviour.GetComponent<T>();
            }
        }

        public static void TryGetInChildren<T>(this Component monoBehaviour, ref T component) where T : Component
        {
            if (component == null)
            {
                component = monoBehaviour.GetComponentInChildren<T>();
            }
        }

        public static void TryGetInParent<T>(this Component monoBehaviour, ref T component) where T : Component
        {
            if (component == null)
            {
                component = monoBehaviour.GetComponentInParent<T>();
            }
        }
    }
}
