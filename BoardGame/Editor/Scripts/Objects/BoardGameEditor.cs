using Celeste.BoardGame;
using CelesteEditor.Tools;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BoardGame.Objects
{
    [CustomEditor(typeof(Celeste.BoardGame.BoardGame))]
    public class BoardGameEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find Board Game Objects"))
            {
                serializedObject.Update();

                string assetFolderPath = AssetUtility.GetAssetFolderPath(target);
                var parentFolder = Directory.GetParent(assetFolderPath);

                SerializedProperty boardGameObjectsProperty = serializedObject.FindProperty("boardGameObjects");

                if (parentFolder != null)
                {
                    string parentFolderName = parentFolder.FullName.Remove(0, Application.dataPath.Length);
                    parentFolderName = $"Assets{parentFolderName}";
                    boardGameObjectsProperty.FindAssets<BoardGameObject>(parentFolderName);
                }
                else
                {
                    boardGameObjectsProperty.FindAssets<BoardGameObject>();
                }

                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }
    }
}
