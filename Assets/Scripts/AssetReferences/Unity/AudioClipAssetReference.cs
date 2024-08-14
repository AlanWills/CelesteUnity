#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using System;
using UnityEngine;

namespace Celeste.Assets.UnityAssetReferences
{
    [Serializable]
    public class AudioClipAssetReference : CelesteAssetReference<AudioClip>
    {
        public AudioClipAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif