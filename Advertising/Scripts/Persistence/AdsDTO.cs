using Celeste.Persistence;
using System;

namespace Celeste.Advertising.Persistence
{
    [Serializable]
    public class AdsManagerDTO : VersionedDTO
    {
        public bool adsTestMode = true;

        public AdsManagerDTO(bool adsTestMode)
        {
            this.adsTestMode = adsTestMode;
        }
    }
}
