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

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void Execute()
        {
            onExecute.Invoke(commandInput.text);
        }

        #region Callbacks

        public void OnDebugCommandExecuted(string output)
        {
            outputText.text = output;
        }

        #endregion
    }
}
