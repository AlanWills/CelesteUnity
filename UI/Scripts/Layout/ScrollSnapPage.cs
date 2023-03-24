using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.UI.Layout
{
    public class ScrollSnapPage : MonoBehaviour
    {
        [SerializeField] private UnityEvent onSelected = new();
        [SerializeField] private UnityEvent onDeselected = new();
        
        public void OnPageChanged(int newPage)
        {
            int thisPageIndex = transform.GetSiblingIndex();

            if (newPage == thisPageIndex)
            {
                onSelected.Invoke();
            }
            else
            {
                onDeselected.Invoke();
            }
        }
    }
}