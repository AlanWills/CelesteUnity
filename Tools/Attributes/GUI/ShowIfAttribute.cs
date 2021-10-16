using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class ShowIfAttribute : PropertyAttribute
    {
        #region Properties and Fields

        public string DependentName { get; private set; }

        #endregion

        public ShowIfAttribute(string dependentName)
        {
            DependentName = dependentName;
        }
    }
}
