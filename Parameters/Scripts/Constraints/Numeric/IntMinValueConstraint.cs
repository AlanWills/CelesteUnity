using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters.Constraints
{
    [CreateAssetMenu(fileName = "Int Min Value", menuName = "Celeste/Constraints/Numeric/Int Min Value")]
    public class IntMinValueConstraint : IntConstraint, IInitializable
    {
        [SerializeField] private IntReference minValue;

        public void Initialize()
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
