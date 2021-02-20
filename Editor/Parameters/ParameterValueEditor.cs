using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters
{
    public class ParameterValueEditor<T> : Editor
    {
        #region Properties and Fields

        protected ParameterValue<T> Parameter { get; private set; }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Parameter = target as ParameterValue<T>;
        }

        #endregion
    }
}
