using Celeste.Parameters;
using Celeste.UI.Skin;
using UnityEngine;

namespace Celeste.UI.Parameters
{
    [CreateAssetMenu(fileName = nameof(UISkinReference), menuName = "Celeste/Parameters/UI/UI Skin Reference")]
    public class UISkinReference : ParameterReference<UISkin, UISkinValue, UISkinReference>
    {
    }
}
