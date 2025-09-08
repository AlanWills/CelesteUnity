using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Input Handler")]
    public class InputHandler : MonoBehaviour, IInputHandler
    {
        #region Properties and Fields

        [HelpBox("Be aware that for touch input based platforms some of these events will never fire, since we do not have the notion of a pointer without input (e.g. desktop platforms have a mouse position even when not down).", HelpBoxMessageType.Warning)]
        [SerializeField] private UnityEvent<InputState> onPointerEnter = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onPointerOver = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onPointerExit = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onPointerFirstDown = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onPointerDown = new UnityEvent<InputState>();
        [SerializeField] private UnityEvent<InputState> onPointerFirstUp = new UnityEvent<InputState>();

        #endregion

        public void OnPointerEnter(InputState inputState)
        {
            onPointerEnter.Invoke(inputState);
        }

        public void OnPointerOver(InputState inputState)
        {
            onPointerOver.Invoke(inputState);
        }

        public void OnPointerExit(InputState inputState)
        {
            onPointerExit.Invoke(inputState);
        }

        public void OnPointerFirstDown(InputState inputState)
        {
            onPointerFirstDown.Invoke(inputState);
        }

        public void OnPointerDown(InputState inputState)
        {
            onPointerDown.Invoke(inputState);
        }

        public void OnPointerFirstUp(InputState inputState)
        {
            onPointerFirstUp.Invoke(inputState);
        }
    }
}
