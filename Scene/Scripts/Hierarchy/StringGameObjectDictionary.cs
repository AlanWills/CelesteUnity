using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Scene.Hierarchy
{
    [CreateAssetMenu(fileName = nameof(StringGameObjectDictionary), menuName = "Celeste/Scene/Game Object Cache")]
    public class StringGameObjectDictionary : DictionaryScriptableObject<string, GameObject>
    {
    }
}
