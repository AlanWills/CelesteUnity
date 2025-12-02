using Celeste.Objects;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    [CreateAssetMenu(fileName = "Float Min Value", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Constraints/Float Min Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class FloatMinValueConstraint : FloatConstraint, IEditorInitializable
    {
        [SerializeField] private FloatReference minValue;

        public void Editor_Initialize()
        {
#if UNITY_EDITOR
            if (minValue == null)
            {
                minValue = CreateInstance<FloatReference>();
                minValue.Value = 0;
                minValue.IsConstant = true;
                minValue.name = $"{name}_MinValue";
                minValue.AddObjectToAsset(this);
            }
#endif
        }

        public override float Constrain(float input)
        {
            return Mathf.Max(minValue.Value, input);
        }
    }
}
