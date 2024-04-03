using Celeste.Debug.Settings;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Debug
{
    [AddComponentMenu("Celeste/Debug/Debug Only")]
    public class DebugOnlyMonoBehaviour : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BoolValue isDebugBuild;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (isDebugBuild == null)
            {
                isDebugBuild = DebugEditorSettings.GetOrCreateSettings().isDebugBuildValue;
            }
        }

        private void Awake()
        {
            if (isDebugBuild == null || !isDebugBuild.Value)
            {
                UnityEngine.Debug.LogFormat("Destroying {0} due to unset or false value IsDebugBuild parameter", gameObject.name);
                GameObject.Destroy(gameObject);
            }
        }

        #endregion
    }
}
