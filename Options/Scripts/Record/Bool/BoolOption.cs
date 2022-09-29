using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(BoolOption), menuName = "Celeste/Options/Bool/Bool Option")]
    public class BoolOption : ScriptableObject
    {
        #region Properties and Fields

        public string DisplayName => displayName;

        public bool Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        [SerializeField] private string displayName;
        [SerializeField] private BoolValue value;

        #endregion

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<bool>> onValueChanged)
        {
            value.AddValueChangedCallback(onValueChanged);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<bool>> onValueChanged)
        {
            value.RemoveValueChangedCallback(onValueChanged);
        }
    }
}
