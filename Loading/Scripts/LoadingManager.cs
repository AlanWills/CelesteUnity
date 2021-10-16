using Celeste.Scene.Events;
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

        #region Callbacks

        public void OnLoadContext(LoadContextArgs loadContextArgs)
        {
            OnContextLoadedEvent onContextLoadedEvent = loadContextArgs.onContextLoaded;
            Context context = loadContextArgs.context;

            disableInput.Invoke();
            loadingScreenUI.SetActive(true);

            StartCoroutine(loadContextArgs.sceneSet.LoadAsync(
                (progress) => progressBar.value = progress,
                () =>
                {
                    onContextLoadedEvent.Invoke(new OnContextLoadedArgs(context));
                    loadingScreenUI.SetActive(false);
                    enableInput.Invoke();
                }));
        }

        #endregion
    }
}