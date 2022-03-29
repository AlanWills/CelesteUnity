using Celeste.Assets;
using Celeste.Debug.Menus;
using System.Collections;
using UnityEngine;

namespace Celeste.Assets.Debug
{
    [AddComponentMenu("Celeste/Debug/Assets/Load Debug Menu Assets")]
    public class LoadDebugMenuAssets : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private DebugMenu debugMenu;

        #endregion

        public bool ShouldLoadAssets()
        {
            IHasAssets hasAssets = debugMenu as IHasAssets;
            return hasAssets != null && hasAssets.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            IHasAssets hasAssets = debugMenu as IHasAssets;
            return hasAssets.LoadAssets();
        }
    }
}
