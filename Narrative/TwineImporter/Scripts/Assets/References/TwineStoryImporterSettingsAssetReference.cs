#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Narrative.TwineImporter.Assets
{
    [Serializable]
    public class TwineStoryImporterSettingsAssetReference : CelesteAssetReference<TwineStoryImporterSettings>
    {
        public TwineStoryImporterSettingsAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif