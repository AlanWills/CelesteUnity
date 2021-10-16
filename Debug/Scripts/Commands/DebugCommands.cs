using Celeste.Events;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Debug.Commands
{
    [AddComponentMenu("Celeste/Debug/Debug Commands")]
    public class DebugCommands : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private List<DebugCommand> registeredCommands = new List<DebugCommand>();
        [SerializeField] StringEvent commandExecutedEvent;

        private StringBuilder output = new StringBuilder();

        #endregion

        #region Callbacks

        public void OnDebugCommandExecuted(string commandText)
        {
            string[] parameters = commandText.Split(' ');
            if (parameters.Length > 0)
            {
                DebugCommand debugCommand = registeredCommands.Find(x => x.CommandName == parameters[0]);
                
                if (debugCommand != null)
                {
                    output.Clear();

                    List<string> commandParams = new List<string>(parameters.Length - 1);
                    for (int i = 1; i < parameters.Length; ++i)
                    {
                        commandParams.Add(parameters[i]);
                    }

                    debugCommand.Execute(commandParams, output);
                    commandExecutedEvent.Invoke(output.ToString());
                }
            }
        }

        #endregion
    }
}
