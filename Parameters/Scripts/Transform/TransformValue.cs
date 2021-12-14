using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "TransformValue", menuName = "Celeste/Parameters/Transform/Transform Value")]
    public class TransformValue : ParameterValue<Transform>
    {
        #region Properties and Fields

        [SerializeField] private TransformEvent onValueChanged;
        protected override ParameterisedEvent<Transform> OnValueChanged => onValueChanged;

        #endregion
    }
}
