using Celeste.Narrative.Backgrounds;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(BackgroundReference), menuName = "Celeste/Parameters/Narrative/Background Reference")]
    public class BackgroundReference : ParameterReference<Background, BackgroundValue, BackgroundReference>
    {
    }
}
