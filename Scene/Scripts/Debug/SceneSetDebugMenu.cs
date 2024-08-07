﻿using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Scene.Catalogue;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GUILayout;

namespace Celeste.Scene.Debug
{
    [CreateAssetMenu(fileName = nameof(SceneSetDebugMenu), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Debug/Scene Set Debug Menu")]
    public class SceneSetDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private SceneSetCatalogue sceneSetCatalogue;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            if (Button("Load Startup Scene"))
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }

            foreach (SceneSet sceneSet in sceneSetCatalogue)
            {
                using (var horizontal = new HorizontalScope())
                {
                    Label(sceneSet.name);

                    if (Button($"Single", ExpandWidth(false)))
                    {
                        CoroutineManager.Instance.StartCoroutine(
                            sceneSet.LoadAsync(LoadSceneMode.Single, (f) => { }, (s) => { }, () => { }));
                    }

                    if (Button($"Additive", ExpandWidth(false)))
                    {
                        CoroutineManager.Instance.StartCoroutine(
                            sceneSet.LoadAsync(LoadSceneMode.Additive, (f) => { }, (s) => { }, () => { }));
                    }
                }
            }
        }

        #endregion
    }
}