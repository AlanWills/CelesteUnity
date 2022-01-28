using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Utils
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount; i > 0; --i)
            {
                GameObject.Destroy(transform.GetChild(i - 1).gameObject);
            }
        }

        public static void DestroyAllChildrenImmediate(this Transform transform)
        {
            for (int i = transform.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(transform.GetChild(i - 1).gameObject);
            }
        }
    }
}
