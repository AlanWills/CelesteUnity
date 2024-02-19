namespace Celeste.Input
{
    public interface IInputHandler
    {
        void OnPointerEnter(InputState inputState);
        void OnPointerOver(InputState inputState);
        void OnPointerExit(InputState inputState);

        void OnPointerFirstDown(InputState inputState);
        void OnPointerDown(InputState inputState);
        void OnPointerFirstUp(InputState inputState);
    }
}