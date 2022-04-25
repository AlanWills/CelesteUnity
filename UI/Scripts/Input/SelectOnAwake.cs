using Celeste.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Select On Awake")]
    public class SelectOnAwake : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Selectable selectable;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref selectable);
        }

        private void Awake()
        {
            selectable.Select();
        }

        #endregion
    }
}
