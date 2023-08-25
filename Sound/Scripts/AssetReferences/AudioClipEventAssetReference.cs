using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Sound.AssetReferences
{
    [Serializable]
    public class AudioClipSettingsEventAssetReference : CelesteAssetReference<AudioClipSettingsEvent>
    {
        public AudioClipSettingsEventAssetReference(string guid) : base(guid)
        {
        }
    }
}
