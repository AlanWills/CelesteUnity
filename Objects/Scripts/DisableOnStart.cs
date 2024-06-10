using UnityEngine;

namespace Celeste.Objects
{
    [AddComponentMenu("Celeste/Objects/Disable On Start")]
    public class DisableOnStart : MonoBehaviour
    {
        [SerializeField] private Behaviour behaviourToDisable;

        private void Start()
        {
            Debug.Assert(behaviourToDisable != null, $"You are attempting to disable a behaviour on Start ({name}), but it is not set.", CelesteLog.Core.WithContext(this));
            if (behaviourToDisable != null)
            {
                behaviourToDisable.enabled = false;
            }
        }
    }
}
