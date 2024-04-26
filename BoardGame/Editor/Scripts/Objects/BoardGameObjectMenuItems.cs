using Celeste;
using Celeste.BoardGame;
using Celeste.BoardGame.Components;
using Celeste.Tools;
using System;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BoardGame.Objects
{
    public static class BoardGameObjectMenuItems
    {
        [MenuItem("Assets/Create/" + CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Preset Objects/Prefab Actor", validate = true, priority = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
        public static bool ValidateCreatePrefabActorBoardGameObjectMenuItem()
        {
            return Selection.objects != null &&
                   Array.Exists(Selection.objects, x => x is GameObject && !(x as GameObject).scene.IsValid());
        }

        [MenuItem("Assets/Create/" + CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Preset Objects/Prefab Actor", validate = false, priority = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
        public static void CreatePrefabActorBoardGameObjectMenuItem()
        {
            for (int i = 0, n = Selection.objects.Length; i < n; i++)
            {
                GameObject prefab = Selection.activeObject as GameObject;

                if (prefab == null ||
                    prefab.scene.IsValid())
                {
                    // Not a project prefab, so we can't use it to create a prefab actor
                    continue;
                }

                string prefabFolder = EditorOnly.GetAssetFolderPath(prefab);

                BoardGameObject boardGameObject = ScriptableObject.CreateInstance<BoardGameObject>();
                boardGameObject.name = $"{prefab.name}BoardGameObject";

                EditorOnly.CreateAssetInFolderAndSave(boardGameObject, prefabFolder);

                PrefabActorBoardGameObjectComponent prefabActor = boardGameObject.CreateComponent<PrefabActorBoardGameObjectComponent>();
                prefabActor.Prefab = prefab;
                prefabActor.CustomiseScale = false;

                AssetDatabase.SaveAssetIfDirty(prefabActor);
                AssetDatabase.SaveAssetIfDirty(boardGameObject);
            }
        }
    }
}
