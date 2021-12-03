using Celeste.Events;
using TMPro;
using UnityEngine;

namespace Celeste.Debug.Commands
{
    [AddComponentMenu("Celeste/Debug/Debug Commands UI")]
    public class DebugCommandsUI : MonoBehaviour
    {
        #region Properties and Fields

        public TMP_InputField commandInput;
        public TextMeshProUGUI outputText;
        public StringEvent onExecute;

        #endregion

        public void Execute()
        {
            onExecute.Invoke(commandInput.text);
        }

        #region Unity Methods

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                Execute();
            }
        }

        #endregion

        #region Activation

        public void TryToggle(MultiTouchEventArgs multiTouchEventArgs)
        {
#if UNITY_ANDROID || UNITY_IOS
            for (int i = 0; i < multiTouchEventArgs.touchCount; ++i)
            {
                if (multiTouchEventArgs.touches[i].phase == TouchPhase.Ended)
                {
                    Toggle();
                    return;
                }
            }
#endif
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        #endregion

        #region Callbacks

        public void OnDebugCommandExecuted(string output)
        {
            outputText.text = output;
        }

        #endregion
    }
}
