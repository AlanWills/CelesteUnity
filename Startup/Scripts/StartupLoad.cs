using Celeste.Loading;
using UnityEngine;

namespace Celeste.Startup
{
    [AddComponentMenu("Celeste/Startup/Startup Load")]
    public class StartupLoad : MonoBehaviour
    {
        #region Properties and Fields

        public LoadJob StartupLoadJob
        {
            set
            {
                startupLoadJob = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(startupLoadJob);
#endif
            }
        }

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