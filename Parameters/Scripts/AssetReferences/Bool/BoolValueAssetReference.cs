using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Parameters.AssetReferences
{
    [Serializable]
    public class BoolValueAssetReference : CelesteAssetReference<BoolValue>
    {
        public BoolValueAssetReference(string guid) : base(guid)
        {
        }
    }
}
