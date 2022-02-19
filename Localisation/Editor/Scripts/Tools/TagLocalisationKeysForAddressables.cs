using Celeste.Localisation.Tools;
using CelesteEditor.Tools;
using System.Collections;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeysForAddressables), menuName = "Celeste/Localisation/Post Import/Tag Localisation Keys For Addressables")]
    public class TagLocalisationKeysForAddressables : LocalisationPostImportStep
    {
        [SerializeField] private string group;
        [SerializeField] private string label;

        public override void Execute(LocalisationPostImportContext localisationPostImportContext)
        {
            foreach (var keyPair in localisationPostImportContext.localisationKeyLookup)
            {
                keyPair.Value.SetAddressableInfo(group);
                keyPair.Value.SetAddressableLabel(group, label, true);
            }
        }
    }
}