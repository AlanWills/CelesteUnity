using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(StringOption), menuName = "Celeste/Options/String/String Option")]
    public class StringOption : ScriptableObject
    {
        #region Properties and Fields

        public string Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        [SerializeField] private StringValue value;

        #endregion

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<string>> onValueChanged)
        {
            value.AddValueChangedCallback(onValueChanged);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<string>> onValueChanged)
        {
            value.RemoveValueChangedCallback(onValueChanged);
        }
    }
}
