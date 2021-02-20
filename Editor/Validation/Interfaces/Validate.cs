using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Validation.Interfaces
{
    public static class Validate
    {
        public static void Show<T>() where T : Object
        {
            for (int i = 0; i < Validator<T>.NumValidationConditions; ++i)
            {
                Debug.LogFormat("Validation Condition: {0} loaded", Validator<T>.GetValidationCondition(i).DisplayName);
            }
        }

        public static void Find<T>() where T : Object
        {
            Validator<T>.FindValidationConditions();
        }

        public static void RunExit<T>() where T : Object
        {
            bool result = RunNoExit<T>();
            
            if (Application.isBatchMode)
            {
                // 0 for success
                // 1 for fail
                EditorApplication.Exit(result ? 0 : 1);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Validation Result",
                    result ?
                        string.Format("All {0} assets passed validation", typeof(T).Name) :
                        string.Format("Some {0} assets failed validation", typeof(T).Name),
                    "OK");
            }
        }

        public static bool RunNoExit<T>() where T : Object
        {
            bool result = true;
            HashSet<string> failedAssets = new HashSet<string>();
            List<string> allAssets = new List<string>();

            foreach (string assetGuid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset == null)
                {
                    result = false;
                    Debug.LogAssertionFormat("Could not find asset with path {0}", assetPath);
                }

                allAssets.Add(asset.name);

                if (!Validator<T>.Validate(asset))
                {
                    failedAssets.Add(asset.name);
                    result = false;
                }
            }

            foreach (string assetName in allAssets)
            {
                if (!failedAssets.Contains(assetName))
                {
                    Debug.LogFormat("{0} passed validation", assetName);

                }
            }

            foreach (string failedAssetName in failedAssets)
            {
                Debug.LogAssertionFormat("{0} failed validation", failedAssetName);
            }

            return result;
        }
    }
}
