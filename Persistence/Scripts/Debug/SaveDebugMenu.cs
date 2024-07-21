using Celeste.Debug.Menus;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Celeste.Persistence
{
    [CreateAssetMenu(fileName = nameof(SaveDebugMenu), menuName = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM + "Debug/Save Debug Menu", order = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM_PRIORITY)]
    public class SaveDebugMenu : DebugMenu
    {
        [SerializeField]
        private List<string> ignoreFiles = new List<string>()
        {
            "global-metadata.dat",
            "System.Data.dll-resources.dat",
            "mscorlib.dll-resources.dat"
        };

        protected override void OnDrawMenu()
        {
            var filesInPersistentData = Directory.GetFiles(Application.persistentDataPath, "*.*", SearchOption.AllDirectories).Where(x => !ignoreFiles.Contains(Path.GetFileName(x)));

            if (GUILayout.Button("Delete All '.dat' Files"))
            {
                foreach (string file in filesInPersistentData)
                {
                    if (file.EndsWith(".dat", System.StringComparison.Ordinal))
                    {
                        File.Delete(file);
                    }
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