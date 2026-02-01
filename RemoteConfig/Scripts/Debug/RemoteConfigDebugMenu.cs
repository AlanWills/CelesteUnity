using Celeste.Debug.Menus;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.RemoteConfig.Debug
{
    [CreateAssetMenu(fileName = nameof(RemoteConfigDebugMenu), order = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM + "Debug Menu")]
    public class RemoteConfigDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private RemoteConfigRecord remoteConfigRecord;

        [NonSerialized] private string remoteConfigJson;

        #endregion

        protected override void OnShowMenu()
        {
            base.OnShowMenu();

            remoteConfigJson = remoteConfigRecord.ToJson();
        }

        protected override void OnDrawMenu()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Fetch"))
                {
                    remoteConfigRecord.FetchData();
                }
                
                if (GUILayout.Button("Refresh Fetched Json"))
                {
                    remoteConfigJson = remoteConfigRecord.ToJson();
                }    
            }

            using (new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(remoteConfigRecord.DataSource != DataSource.Disabled))
                {
                    if (GUILayout.Button("Disable"))
                    {
                        remoteConfigRecord.DataSource = DataSource.Disabled;
                        remoteConfigJson = remoteConfigRecord.ToJson();
                    }
                }

#if UNITY_REMOTE_CONFIG
                using (new GUIEnabledScope(remoteConfigRecord.DataSource != DataSource.Unity))
                {
                    if (GUILayout.Button("Unity"))
                    {
                        remoteConfigRecord.DataSource = DataSource.Unity;
                        remoteConfigJson = remoteConfigRecord.ToJson();
                    }
                }
#endif
            }

            using (Section("Fetched Json"))
            {
                GUILayout.Label(remoteConfigJson);
            }
        }
    }
}
