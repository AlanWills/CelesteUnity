using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "GameObjectReference", menuName = "Celeste/Parameters/Game Object/GameObject Reference")]
    public class GameObjectReference : ParameterReference<GameObject, GameObjectValue, GameObjectReference>
    {
    }
}
