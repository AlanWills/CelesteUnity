using UnityEngine;

namespace Celeste.Objects
{
    [AddComponentMenu("Celeste/Objects/Set Inactive On Awake")]
    public class SetInactiveOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
