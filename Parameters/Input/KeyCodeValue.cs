using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = "New KeyCodeValue", menuName = "Celeste/Parameters/Input/KeyCode Value")]
    public class KeyCodeValue : ParameterValue<KeyCode>
    {
    }
}
