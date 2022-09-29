using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(IntOption), menuName = "Celeste/Options/Int/Int Option")]
    public class IntOption : ScriptableObject
    {
        #region Properties and Fields

        public int Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        [SerializeField] private IntValue value;

        #endregion

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<int>> onValueChanged)
        {
            value.AddValueChangedCallback(onValueChanged);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<int>> onValueChanged)
        {
            value.RemoveValueChangedCallback(onValueChanged);
        }
    }
}
