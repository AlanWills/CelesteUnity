using Celeste.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Platform.Data
{
    [CreateAssetMenu(fileName = nameof(ScriptingDefineSymbols), menuName = "Celeste/Platform/Scripting Define Symbols")]
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
    }
}
