using System;

namespace Celeste.Advertising.Persistence
{
    [Serializable]
    public class AdsManagerDTO
    {
        public bool adsTestMode = true;

        public AdsManagerDTO(bool adsTestMode)
        {
            this.adsTestMode = adsTestMode;
        }
    }
}
