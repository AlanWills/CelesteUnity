using Celeste.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.Features.Persistence
{
    [Serializable]
    public class FeatureManagerDTO : VersionedDTO
    {
        public List<int> enabledFeatures;

        public FeatureManagerDTO(List<int> enabledFeatures)
        {
            this.enabledFeatures = new List<int>(enabledFeatures);
        }
    }
}
