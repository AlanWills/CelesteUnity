using Celeste.Debug.Tools;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(
        fileName = nameof(PlayerConnectionDebugMenu), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Player Connection Debug Menu",  
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class PlayerConnectionDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private PlayerConnectionRecord playerConnectionRecord;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Is Connected: {playerConnectionRecord.IsConnected}");
            GUILayout.Label($"Is Setup: {playerConnectionRecord.IsSetup}");

            using (new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(!playerConnectionRecord.IsConnected))
                {
                    if (GUILayout.Button("Setup Connection"))
                    {
                        playerConnectionRecord.SetupConnectionToEditor();
                    }
                }
                
                using (new GUIEnabledScope(playerConnectionRecord.IsConnected))
                {
                    if (GUILayout.Button("Tear Down Connection"))
                    {
                        playerConnectionRecord.TearDownConnectionToEditor();
                    }
                }
            }
        }
    }
}