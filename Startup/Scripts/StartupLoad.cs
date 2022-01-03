using Celeste.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Startup
{
    [AddComponentMenu("Celeste/Startup/Startup Load")]
    public class StartupLoad : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private SceneSet sceneSetToLoad;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            StartCoroutine(sceneSetToLoad.LoadAsync(LoadSceneMode.Single, (f) => { }, () => { }));
        }

        #endregion
    }
}