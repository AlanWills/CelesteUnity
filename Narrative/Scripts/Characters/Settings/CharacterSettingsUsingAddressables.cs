#if USE_ADDRESSABLES
using Celeste.Narrative.Assets.References;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Characters.Settings
{
    [CreateAssetMenu(fileName = nameof(CharacterSettingsUsingAddressables), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Characters/Character Settings Using Addressables", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class CharacterSettingsUsingAddressables : CharacterSettings
    {
        #region Properties and Fields
        
        [SerializeField] private CharacterCustomisationCatalogueAssetReference customisationCatalogue;

        [NonSerialized] private bool loaded = false;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return loaded;
        }

        public override IEnumerator LoadAssets()
        {
            yield return customisationCatalogue.LoadAssetAsync();
        }

        public override T FindCustomisationByGuid<T>(int guid)
        {
            return customisationCatalogue.Asset.FindByGuid<T>(guid);
        }
    }
}
#endif