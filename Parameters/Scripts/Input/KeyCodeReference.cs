using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = "New KeyCodeReference", menuName = "Celeste/Parameters/Input/KeyCode Reference")]
    public class KeyCodeReference : ParameterReference<KeyCode, KeyCodeValue, KeyCodeReference>
    {
    }
}
