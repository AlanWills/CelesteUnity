using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Options.Debug
{
    [CreateAssetMenu(fileName = nameof(OptionsDebugMenu), menuName = "Celeste/Options/Debug/Options Debug Menu")]
    public class OptionsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private OptionsRecord options;

        #endregion

        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(true)))
            {
                options.ResetAll();
            }
        }
    }
}
