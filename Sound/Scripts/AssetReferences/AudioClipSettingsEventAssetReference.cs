using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Sound.AssetReferences
{
    [Serializable]
    public class AudioClipEventAssetReference : CelesteAssetReference<AudioClipEvent>
    {
        public AudioClipEventAssetReference(string guid) : base(guid)
        {
        }
    }
}
