using Celeste.Objects;
using UnityEngine;

namespace CelesteEditor.Platform.Data
{
    [CreateAssetMenu(fileName = nameof(AddressableGroupNames), menuName = "Celeste/Platform/Addressable Group Names")]
    public class AddressableGroupNames : ListScriptableObject<string>
    {
        public bool Contains(string groupName)
        {
            return FindItem(x => string.CompareOrdinal(x, groupName) == 0) != null;
        }
    }
}
