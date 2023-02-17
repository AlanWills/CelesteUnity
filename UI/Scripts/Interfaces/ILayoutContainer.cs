using UnityEngine;

namespace Celeste.UI
{
    public interface ILayoutContainer
    {
        void OnChildAdded(GameObject gameObject);
        void OnChildRemoved(GameObject gameObject);
    }
}
