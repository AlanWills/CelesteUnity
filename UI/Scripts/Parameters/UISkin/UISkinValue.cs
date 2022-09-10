using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI.Skin;
using UnityEngine;

namespace Celeste.UI.Parameters
{
    [CreateAssetMenu(fileName = nameof(UISkinValue), menuName = "Celeste/Parameters/UI/UI Skin Value")]
    public class UISkinValue : ParameterValue<UISkin, UISkinValueChangedEvent>
    {
    }
}