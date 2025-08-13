#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using Celeste.Events;
using System;

namespace Celeste.Narrative.Assets.References
{
    [Serializable]
    public class BackgroundEventAssetReference : CelesteAssetReference<SetBackgroundEvent>
    {
        public BackgroundEventAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif