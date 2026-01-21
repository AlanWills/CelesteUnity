using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Celeste.Objects
{
    [CreateAssetMenu(fileName = nameof(VariableStore), menuName = CelesteMenuItemConstants.OBJECTS_MENU_ITEM, order = CelesteMenuItemConstants.OBJECTS_MENU_ITEM_PRIORITY)]
    public class VariableStore : ScriptableObject
    {
        #region Properties and Fields

        [NonSerialized] private Dictionary<string, bool> boolVariables = new(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<string, int> intVariables = new(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<string, float> floatVariables = new(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<string, string> stringVariables = new(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<string, object> objectVariables = new(StringComparer.Ordinal);
        [NonSerialized] private Dictionary<string, Object> unityObjectVariables = new(StringComparer.Ordinal);

        #endregion
    }
}