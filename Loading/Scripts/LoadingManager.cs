using Celeste.Scene.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DnD.Core.Loading
{
    [AddComponentMenu("DnD/Core/Loading/Loading Manager")]
    public class LoadingManager : MonoBehaviour
    {
        #region Properties and Fields

        [Header("UI")]
        [SerializeField] private GameObject loadingScreenUI;
        [SerializeField] private Slider progressBar;

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event disableInput;
        [SerializeField] private Celeste.Events.Event enableInput;

        #endregion

        private IEnumerator LoadContext(LoadContextArgs loadContextArgs)
        {
            disableInput.Invoke();
            loadingScreenUI.SetActive(true);

            yield return loadContextArgs.sceneSet.LoadAsync(
                (progress) => progressBar.value = progress,
                () => { });

            loadContextArgs.onContextLoaded.Invoke(new OnContextLoadedArgs(loadContextArgs.context));
            loadingScreenUI.SetActive(false);
            enableInput.Invoke();
        }

        #region Callbacks

        public void OnLoadContext(LoadContextArgs loadContextArgs)
        {
            StartCoroutine(LoadContext(loadContextArgs));
        }

        #endregion
    }
}