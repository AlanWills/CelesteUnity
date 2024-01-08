using Celeste.Objects;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(AddressableGroupNames), menuName = "Celeste/Build System/Addressable Group Names")]
    public class AddressableGroupNames : ListScriptableObject<string>
    {
        #region Properties and Fields

        [SerializeField] private bool useAllCreatedAddressableGroups = true;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return;
            }

            if (useAllCreatedAddressableGroups)
            {
                UnityEngine.Debug.Assert(AddressableAssetSettingsDefaultObject.Settings != null, $"Addressable Settings was null, which is weird because we thinks it exists...");
                UnityEngine.Debug.Assert(AddressableAssetSettingsDefaultObject.Settings.groups != null, $"Addressable Settings groups were null, which is weird because we thinks it exists...");

                foreach (var group in AddressableAssetSettingsDefaultObject.Settings.groups)
                {
                    // Don't include the built in data group
                    if (string.CompareOrdinal(group.name, "Built In Data") != 0 && 
                        FindItem(x => string.CompareOrdinal(group.name, x) == 0) == null)
                    {
                        AddItem(group.name);
                        EditorUtility.SetDirty(this);
                    }
                }
            }
        }

        #endregion

        public bool Contains(string groupName)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                return false;
            }

            return useAllCreatedAddressableGroups ? 
                AddressableAssetSettingsDefaultObject.Settings.FindGroup(groupName) != null : 
                FindItem(x => string.CompareOrdinal(x, groupName) == 0) != null;
        }
    }
}
