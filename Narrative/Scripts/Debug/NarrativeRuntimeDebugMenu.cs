using Celeste.Debug.Menus;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Narrative.Debug
{
    [CreateAssetMenu(fileName = nameof(NarrativeRuntimeDebugMenu), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Debug/Narrative Runtime Debug Menu", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeRuntimeDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private Celeste.Events.Event debugNext;
        [SerializeField] private Celeste.Events.Event debugRestart;
        [SerializeField] private Celeste.Events.Event debugFinish;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            using (new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(debugNext != null))
                {
                    if (GUILayout.Button("Next"))
                    {
                        debugNext.Invoke();
                    }
                }

                using (new GUIEnabledScope(debugRestart != null))
                {
                    if (GUILayout.Button("Restart"))
                    {
                        debugRestart.Invoke();
                    }
                }

                using (new GUIEnabledScope(debugFinish != null))
                {
                    if (GUILayout.Button("Finish"))
                    {
                        debugFinish.Invoke();
                    }
                }
            }
        }
    }
}