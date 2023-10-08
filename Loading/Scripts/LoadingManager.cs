using Celeste.Loading;
using Celeste.Scene.Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private TextMeshProUGUI loadingInfo;

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event disableInput;
        [SerializeField] private Celeste.Events.Event enableInput;

        #endregion

        private IEnumerator LoadContext(LoadContextArgs loadContextArgs)
        {
            disableInput.Invoke();
            loadingScreenUI.SetActive(true);
            loadingInfo.text = "Loading...";

            yield return loadContextArgs.sceneSet.LoadAsync(
                LoadSceneMode.Single,
                (progress) => progressBar.value = progress,
                (s) => 
                {
                    if (loadContextArgs.showOutputOnLoadingScreen)
                    {
                        loadingInfo.text = s;
                    }
                },
                () => { });

            if (loadContextArgs.onContextLoaded != null)
            {
                loadContextArgs.onContextLoaded.Invoke(new OnContextLoadedArgs(loadContextArgs.context));
            }

            loadingScreenUI.SetActive(false);
            enableInput.Invoke();
        }

        private IEnumerator ExecuteLoadJob(LoadJob loadJob)
        {
            disableInput.Invoke();
            loadingScreenUI.SetActive(true);
            loadingInfo.text = "Loading...";

            yield return loadJob.Execute(
                (progress) => progressBar.value = progress, 
                (s) => 
                {
                    if (loadJob.ShowOutputInLoadingScreen)
                    {
                        loadingInfo.text = s;
                    }
                });

            loadingScreenUI.SetActive(false);
            enableInput.Invoke();
        }

        #region Callbacks

        public void OnLoadContext(LoadContextArgs loadContextArgs)
        {
            StartCoroutine(LoadContext(loadContextArgs));
        }

        public void OnExecuteLoadJob(LoadJob loadJob)
        {
            StartCoroutine(ExecuteLoadJob(loadJob));
        }

        #endregion
    }
}