using Celeste.Debug.Tools;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(
        fileName = nameof(EditorConnectionDebugMenu), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Editor Connection Debug Menu",  
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class EditorConnectionDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private EditorConnectionRecord editorConnectionRecord;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Connection Setup: {editorConnectionRecord.ConnectionSetup}");

            using (new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(!editorConnectionRecord.ConnectionSetup))
                {
                    if (GUILayout.Button("Setup Editor Connection"))
                    {
                        editorConnectionRecord.SetupEditorConnection();
                    }
                }
                
                using (new GUIEnabledScope(editorConnectionRecord.ConnectionSetup))
                {
                    if (GUILayout.Button("Teardown Editor Connection"))
                    {
                        editorConnectionRecord.TeardownEditorConnection();
                    }
                }
            }
            
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label($"Num Connected Players: {editorConnectionRecord.NumConnectedPlayers}");

                if (GUILayout.Button("Ping All"))
                {
                    editorConnectionRecord.PingAll();
                }
            }

            foreach (var player in editorConnectionRecord.ConnectedPlayers)
            {
                GUILayout.Label($"Player Id: {player}");
            }
        }
    }
}