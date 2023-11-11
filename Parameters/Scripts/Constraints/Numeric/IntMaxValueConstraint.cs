using Celeste.Objects;
using System.Diagnostics;
using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    [CreateAssetMenu(fileName = "Int Max Value", menuName = "Celeste/Constraints/Numeric/Int Max Value")]
    public class IntMaxValueConstraint : IntConstraint, IEditorInitializable
    {
        [SerializeField] private IntReference maxValue;

        public void Editor_Initialize()
        {
#if UNITY_EDITOR
            if (maxValue == null)
            {
                maxValue = CreateInstance<IntReference>();
                maxValue.Value = int.MaxValue;
                maxValue.IsConstant = true;
                maxValue.name = $"{name}_MaxValue";

                UnityEditor.AssetDatabase.AddObjectToAsset(maxValue, this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
#endif
        }

        public override int Constrain(int input)
        {
            return Mathf.Min(maxValue.Value, input);
        }
    }
}
