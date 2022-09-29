using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(FloatOption), menuName = "Celeste/Options/Float/Float Option")]
    public class FloatOption : ScriptableObject
    {
        #region Properties and Fields

        public float Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        [SerializeField] private FloatValue value;

        #endregion

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<float>> onValueChanged)
        {
            value.AddValueChangedCallback(onValueChanged);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<float>> onValueChanged)
        {
            value.RemoveValueChangedCallback(onValueChanged);
        }
    }
}
