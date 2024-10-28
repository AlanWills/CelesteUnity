using Celeste.Loading;
using Celeste.Scene.Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Celeste.Core.Loading
{
    [AddComponentMenu("Celeste/Loading/Loading Manager")]
    public class LoadingManager : MonoBehaviour
    {
        #region Properties and Fields

        [Header("UI")]
        [SerializeField] private GameObject loadingScreenUI;
        [SerializeField] private GameObject loadingOverlayUI;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI loadingInfo;
        [SerializeField] private TextMeshProUGUI loadingPercentage;
        [SerializeField] private string loadingPercentageFormat = "Loading... {0}%";

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event disableInput;
        [SerializeField] private Celeste.Events.Event enableInput;

        #endregion

        private IEnumerator LoadContext(LoadContextArgs loadContextArgs)
        {
            disableInput.Invoke();
            loadingScreenUI.SetActive(true);
            loadingInfo.text = "";

            yield return loadContextArgs.sceneSet.LoadAsync(
                LoadSceneMode.Single,
                SetProgress,
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

        private IEnumerator ExecuteLoadJobWithScreen(LoadJob loadJob)
        {
            disableInput.Invoke();
            loadingScreenUI.SetActive(true);
            loadingInfo.text = "";

            yield return loadJob.Execute(
                SetProgress,
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

        private IEnumerator ExecuteLoadJobWithOverlay(LoadJob loadJob)
        {
            disableInput.Invoke();
            loadingOverlayUI.SetActive(true);

            yield return loadJob.Execute(
                SetProgress,
                (s) => { });

            loadingOverlayUI.SetActive(false);
            enableInput.Invoke();
        }

        private IEnumerator ExecuteLoadJobSilently(LoadJob loadJob)
        {
            disableInput.Invoke();

            yield return loadJob.Execute(
                SetProgress,
                (s) => { });

            enableInput.Invoke();
        }

        private void SetProgress(float progress)
        {
            progressBar.value = progress;
            loadingPercentage.text = string.Format(loadingPercentageFormat, Mathf.RoundToInt(progress * 100));
        }

        #region Callbacks

        public void OnLoadContext(LoadContextArgs loadContextArgs)
        {
            StartCoroutine(LoadContext(loadContextArgs));
        }

        public void OnExecuteLoadJobWithScreen(LoadJob loadJob)
        {
            StartCoroutine(ExecuteLoadJobWithScreen(loadJob));
        }

        public void OnExecuteLoadJobWithOverlay(LoadJob loadJob)
        {
            StartCoroutine(ExecuteLoadJobWithOverlay(loadJob));
        }

        public void OnExecuteLoadJobSilently(LoadJob loadJob)
        {
            StartCoroutine(ExecuteLoadJobSilently(loadJob));
        }

        #endregion
    }
}