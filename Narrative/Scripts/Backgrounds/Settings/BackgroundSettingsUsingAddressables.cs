#if USE_ADDRESSABLES
using Celeste.Narrative.Assets.References;
using Celeste.Narrative.Parameters;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Narrative.Backgrounds.Settings
{
    [CreateAssetMenu(fileName = nameof(BackgroundSettingsUsingAddressables), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Backgrounds/Background Settings Using Addressables", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class BackgroundSettingsUsingAddressables : BackgroundSettings
    {
        #region Properties and Fields

        [SerializeField] private BackgroundCatalogueAssetReference backgroundCatalogue;
        
        [Header("Events")]
        [SerializeField] private BackgroundEventAssetReference setBackgroundEvent;

        [NonSerialized] private bool loaded;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return loaded;
        }

        public override IEnumerator LoadAssets()
        {
            yield return backgroundCatalogue.LoadAssetAsync();
            yield return setBackgroundEvent.LoadAssetAsync();

            loaded = true;
        }

        public override Background FindBackgroundByGuid(int guid)
        {
            return backgroundCatalogue.Asset.FindByGuid(guid);
        }

        public override void AddSetBackgroundListener(UnityAction<Background> background)
        {
            setBackgroundEvent.Asset.AddListener(background);
        }

        public override void RemoveSetBackgroundListener(UnityAction<Background> background)
        {
            setBackgroundEvent.Asset.RemoveListener(background);
        }
    }
}
#endif