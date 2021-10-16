using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Debug.Commands
{
    public abstract class DebugCommand : ScriptableObject
    {
        #region Properties and Fields

        public string CommandName
        {
            get { return commandName; }
        }
        
        [SerializeField] private string commandName;

        #endregion

        public abstract bool Execute(List<string> parameters, StringBuilder output);
    }
}
