using Celeste.Events;
using Celeste.Narrative.Characters;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(BackgroundValue), menuName = "Celeste/Parameters/Narrative/Background Value")]
    public class BackgroundValue : ParameterValue<Background>
    {
        [SerializeField] private BackgroundEvent onValueChanged;
        protected override ParameterisedEvent<Background> OnValueChanged => onValueChanged;
    }
}
