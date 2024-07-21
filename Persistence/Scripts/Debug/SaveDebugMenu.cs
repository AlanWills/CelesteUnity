using Celeste.Debug.Menus;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence
{
    [CreateAssetMenu(fileName = nameof(SaveDebugMenu), menuName = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM + "Debug/Save Debug Menu", order = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM_PRIORITY)]
    public class SaveDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            string[] filesInPersistentData = Directory.GetFiles(Application.persistentDataPath, "*.dat", SearchOption.AllDirectories);

            if (GUILayout.Button("Delete All"))
            {
                foreach (string file in filesInPersistentData)
                {
                    File.Delete(file);
                }
            }

            foreach (string file in filesInPersistentData)
            {
                using (new GUILayout.HorizontalScope())
                {
                    string fileName = Path.GetFileName(file);
                    GUILayout.Label(fileName);

                    if (GUILayout.Button("Delete"))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}