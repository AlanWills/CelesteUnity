using Celeste.BoardGame;
using Celeste.BoardGame.Components;
using CelesteEditor.Components;
using CelesteEditor.Tools;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BoardGame.Objects
{
    [CustomEditor(typeof(Celeste.BoardGame.BoardGame))]
    public class BoardGameEditor : ComponentContainerUsingSubAssetsEditor<BoardGameComponent>
    {
        #region Properties and Fields

        protected override Type[] AllComponentTypes => BoardGameEditorConstants.AllBoardGameComponentTypes;
        protected override string[] AllComponentDisplayNames => BoardGameEditorConstants.AllBoardGameComponentDisplayNames;

        #endregion

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
