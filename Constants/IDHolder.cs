using UnityEngine;

namespace Celeste.Constants
{
    [AddComponentMenu("Celeste/Constants/ID Holder")]
    public class IDHolder : MonoBehaviour
    {
        [SerializeField] private ID id;

        public bool HasID(ID id)
        {
            return this.id == id;
        }
    }
}
