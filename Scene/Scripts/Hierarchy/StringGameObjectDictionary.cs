using Celeste.Objects;
using UnityEngine;

namespace Celeste.Scene.Hierarchy
{
    [CreateAssetMenu(fileName = nameof(StringGameObjectDictionary), menuName = "Celeste/Scene/String Game Object Dictionary")]
    public class StringGameObjectDictionary : DictionaryScriptableObject<string, GameObject>
    {
    }
}
