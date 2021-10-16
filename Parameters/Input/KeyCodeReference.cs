using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = "New KeyCodeReference", menuName = "Celeste/Parameters/Input/KeyCode Reference")]
    public class KeyCodeReference : ParameterReference<KeyCode, KeyCodeValue, KeyCodeReference>
    {
    }
}
