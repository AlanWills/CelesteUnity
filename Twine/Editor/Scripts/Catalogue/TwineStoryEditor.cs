using Celeste.Tools;
using Celeste.Twine;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Twine.Database
{
    [CustomEditor(typeof(TwineStory))]
    public class TwineStoryEditor : Editor
    {
        #region Create Menu Item

        [MenuItem("Assets/Create/Celeste/Twine/Twine Story")]
        public static void CreateTwineStoryMenuItem()
        {
            Object selectionObject = Selection.activeObject;
            if (selectionObject != null)
            {
                string folder = EditorOnly.GetAssetFolderPath(selectionObject);
                string path = Path.Combine(folder, $"{nameof(TwineStory)}.{TwineStory.FILE_EXTENSION}");
                File.WriteAllText(path, "");

                AssetDatabase.Refresh();
                Debug.Log($"Created {nameof(TwineStory)} at {path}.");
            }
            else
            {
                Debug.LogError($"Failed to create {nameof(TwineStory)} due to no selected folder.");
            }
        }

        #endregion
    }
}
