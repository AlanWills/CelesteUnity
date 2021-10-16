using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters.Rendering
{
    [CreateAssetMenu(fileName = "SpriteReference", menuName = "Celeste/Parameters/Rendering/Sprite Reference")]
    public class SpriteReference : ParameterReference<Sprite, SpriteValue, SpriteReference>
    {
    }
}
