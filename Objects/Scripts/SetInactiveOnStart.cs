using UnityEngine;

namespace Celeste.Objects
{
    [AddComponentMenu("Celeste/Objects/Set Inactive On Start")]
    public class SetInactiveOnStart : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
