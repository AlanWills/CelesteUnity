using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryValue), menuName = "Celeste/Parameters/Localisation/Localisation Key Category Value")]
    public class LocalisationKeyCategoryValue : ParameterValue<LocalisationKeyCategory>
    {
        protected override ParameterisedEvent<LocalisationKeyCategory> OnValueChanged => null;
    }
}
