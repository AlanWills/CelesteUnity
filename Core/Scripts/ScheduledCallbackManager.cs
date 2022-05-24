using UnityEngine;

namespace Celeste.Core
{
    [AddComponentMenu("Celeste/Core/Scheduled Callback Manager")]
    public class ScheduledCallbackManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ScheduledCallbacks scheduledCallbacks;

        #endregion

        public void Update()
        {
            scheduledCallbacks.Update();
        }
    }
}
