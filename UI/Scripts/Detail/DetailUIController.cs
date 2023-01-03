using UnityEngine;

namespace Celeste.UI
{
    public abstract class DetailUIController : MonoBehaviour
    {
        public abstract bool IsValidFor(IDetailContext detailContext);
        public abstract void Show(IDetailContext detailContext);
        public abstract void Hide();
    }
}
