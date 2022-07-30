using Celeste.Objects;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(AddressableGroupNames), menuName = "Celeste/Build System/Addressable Group Names")]
    public class AddressableGroupNames : ListScriptableObject<string>
    {
        public bool Contains(string groupName)
        {
            return FindItem(x => string.CompareOrdinal(x, groupName) == 0) != null;
        }
    }
}
