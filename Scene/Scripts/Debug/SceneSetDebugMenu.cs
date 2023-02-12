using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Scene;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GUILayout;

namespace Celeste.Scene.Debug
{
    [CreateAssetMenu(fileName = nameof(SceneSetDebugMenu), menuName = "Celeste/Scene/Debug/Scene Set Debug Menu")]
    public class SceneSetDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private List<SceneSet> sceneSets = new List<SceneSet>();

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = sceneSets.Count; i < n; ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    SceneSet sceneSet = sceneSets[i];

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