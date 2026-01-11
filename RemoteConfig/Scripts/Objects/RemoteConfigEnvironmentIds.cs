using UnityEngine;

namespace Celeste.RemoteConfig.Objects
{
    [CreateAssetMenu(fileName = nameof(RemoteConfigEnvironmentIds), menuName = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM + "Remote Config Environment Ids", order = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM_PRIORITY)]
    public class RemoteConfigEnvironmentIds : ScriptableObject
    {
        #region Properties and Fields
        
#if UNITY_REMOTE_CONFIG
        [Header("Unity")]
        [SerializeField] private string unityDevelopmentEnvironmentId;
        [SerializeField] private string unityProductionEnvironmentId;
#endif
        
        #endregion
        
        public string GetEnvironmentId(DataSource dataSource, bool isDebugBuild)
        {
            switch (dataSource)
            {
                case DataSource.Disabled:
                    return string.Empty;

#if UNITY_REMOTE_CONFIG
                case DataSource.Unity:
                    return isDebugBuild ? unityDevelopmentEnvironmentId : unityProductionEnvironmentId;
#else
                    return string.Empty;
#endif
                default:
                    UnityEngine.Debug.LogAssertion(
                        $"Unhandled {nameof(DataSource)} value {dataSource} when obtaining environment Id in {nameof(RemoteConfigManager)} {name}.");
                    return string.Empty;
            }
        }

    }
}