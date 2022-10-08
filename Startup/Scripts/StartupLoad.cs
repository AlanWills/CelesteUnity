using Celeste.Loading;
using Celeste.Scene;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Startup
{
    [AddComponentMenu("Celeste/Startup/Startup Load")]
    public class StartupLoad : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LoadJob startupLoadJob;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            StartCoroutine(startupLoadJob.Execute((f) => { }, (s) => { }));
        }

        #endregion
    }
}