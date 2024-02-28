using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    [CreateAssetMenu(fileName = "Int Min Value", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Constraints/Int Min Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class IntMinValueConstraint : IntConstraint, IEditorInitializable
    {
        [SerializeField] private IntReference minValue;

        public void Editor_Initialize()
        {
#if UNITY_EDITOR
            if (minValue == null)
            {
                minValue = CreateInstance<IntReference>();
                minValue.Value = 0;
                minValue.IsConstant = true;
                minValue.name = $"{name}_MinValue";

                UnityEditor.AssetDatabase.AddObjectToAsset(minValue, this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
#endif
        }

        public override int Constrain(int input)
        {
            return Mathf.Max(minValue.Value, input);
        }
    }
}
