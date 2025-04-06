using Celeste;
using Celeste.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(ScriptingDefineSymbols), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Scripting Define Symbols",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class ScriptingDefineSymbols : ListScriptableObject<string>
    {
        public string[] ToArray()
        {
            List<string> list = new List<string>(NumItems);

            for (int i = 0, n = NumItems; i < n; i++)
            {
                list.Add(GetItem(i));
            }

            return list.ToArray();
        }

        public void AddDefaultDebugSymbols()
        {
            AddItem("DEVELOPMENT");
            AddItem("INDEX_CHECKS");
            AddItem("NULL_CHECKS");
            AddItem("COMPONENT_CHECKS");
            AddItem("DATA_CHECKS");
            AddItem("KEY_CHECKS");
            AddItem("ALLOCATOR_CHECKS");
            AddItem("ENABLE_INPUT_SYSTEM");
        }

        public void AddDefaultReleaseSymbols()
        {
            AddItem("ENABLE_INPUT_SYSTEM");
        }
    }
}
