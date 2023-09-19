using Celeste.Debug.Menus;
using System;
using UnityEngine;

namespace Celeste.RemoteConfig.Debug
{
    [CreateAssetMenu(fileName = nameof(RemoteConfigDebugMenu), menuName = "Celeste/Remote Config/Debug Menu")]
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

            GUILayout.Label(remoteConfigJson);
        }
    }
}
