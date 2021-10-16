using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Objects
{
    public static class ScriptableObjectExtensions
    {
        public static T AddSubAsset<T>(
            this ScriptableObject scriptableObject, 
            Type subAssetType, 
            List<T> listToAddTo) where T : ScriptableObject
        {
            T subAsset = ScriptableObject.CreateInstance(subAssetType) as T;
            subAsset.name = subAssetType.Name;
            Debug.AssertFormat(subAsset != null, "Failed to create sub asset of type {0}.  Inputting type is {1}", typeof(T).Name, subAssetType.Name);

            subAsset.hideFlags = HideFlags.HideInHierarchy;
            listToAddTo.Add(subAsset);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(subAsset, scriptableObject);
            UnityEditor.EditorUtility.SetDirty(scriptableObject);
#endif
            return subAsset;
        }

        public static void RemoveSubAsset<T>(
            this ScriptableObject scriptableObject, 
            int index,
            List<T> listToRemoveFrom) where T : ScriptableObject
        {
            T subAsset = listToRemoveFrom.Get(index);
            listToRemoveFrom.RemoveAt(index);

#if UNITY_EDITOR
            if (subAsset != null)
            {
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(subAsset);
            }
            UnityEditor.EditorUtility.SetDirty(scriptableObject);
#endif
        }
    }
}
