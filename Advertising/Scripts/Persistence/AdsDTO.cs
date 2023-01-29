using System;

namespace Celeste.Advertising.Persistence
{
    [Serializable]
    public class AdsDTO
    {
        public bool adsTestMode = true;

        public AdsDTO(bool adsTestMode)
        {
            this.adsTestMode = adsTestMode;
        }
    }
}
