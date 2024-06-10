using UnityEngine;

namespace Celeste.Objects
{
    [AddComponentMenu("Celeste/Objects/Disable On Awake")]
    public class DisableOnAwake : MonoBehaviour
    {
        [SerializeField] private Behaviour behaviourToDisable;

        private void Awake()
        {
            Debug.Assert(behaviourToDisable != null, $"You are attempting to disable a behaviour on Awake ({name}), but it is not set.", CelesteLog.Core.WithContext(this));
            if (behaviourToDisable != null)
            {
                behaviourToDisable.enabled = false;
            }
        }
    }
}
