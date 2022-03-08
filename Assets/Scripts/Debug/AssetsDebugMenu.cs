using Celeste.Debug.Menus;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Assets.Debug
{
    [CreateAssetMenu(fileName = nameof(AssetsDebugMenu), menuName = "Celeste/Assets/Debug/Assets Debug Menu")]
    public class AssetsDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Clear Cache"))
            {
                CachingUtility.ClearCache();
            }
        }
    }
}
