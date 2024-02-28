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
            if (GUILayout.Button("Refresh"))
            {
                remoteConfigJson = remoteConfigRecord.ToJson();
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

                using (new GUIEnabledScope(remoteConfigRecord.DataSource != DataSource.Unity))
                {
                    if (GUILayout.Button("Unity"))
                    {
                        remoteConfigRecord.DataSource = DataSource.Unity;
                        remoteConfigJson = remoteConfigRecord.ToJson();
                    }
                }
            }

            GUILayout.Label(remoteConfigJson);
        }
    }
}
