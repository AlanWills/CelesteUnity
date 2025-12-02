using Celeste.Objects;
using System.Diagnostics;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    [CreateAssetMenu(fileName = "Float Max Value", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Constraints/Float Max Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class FloatMaxValueConstraint : FloatConstraint, IEditorInitializable
    {
        [SerializeField] private FloatReference maxValue;

        public void Editor_Initialize()
        {
#if UNITY_EDITOR
            if (maxValue == null)
            {
                maxValue = CreateInstance<FloatReference>();
                maxValue.Value = float.MaxValue;
                maxValue.IsConstant = true;
                maxValue.name = $"{name}_MaxValue";
                maxValue.AddObjectToAsset(this);
            }
#endif
        }

        public override float Constrain(float input)
        {
            return Mathf.Min(maxValue.Value, input);
        }
    }
}
