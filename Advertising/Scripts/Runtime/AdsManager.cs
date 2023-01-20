using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Advertising
{
    public class AdsManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BoolValue adTestMode;
        [SerializeField] private AdRecord adRecord;

        #endregion

        #region Unity Methods

        private void Start()
        {
            adRecord.Initialize(adTestMode.Value);
        }

        #endregion
    }
}
