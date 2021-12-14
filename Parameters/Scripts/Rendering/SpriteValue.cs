using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters.Rendering
{
    [CreateAssetMenu(fileName = "SpriteValue", menuName = "Celeste/Parameters/Rendering/Sprite Value")]
    public class SpriteValue : ParameterValue<Sprite>
    {
        protected override ParameterisedEvent<Sprite> OnValueChanged => null;
    }
}
