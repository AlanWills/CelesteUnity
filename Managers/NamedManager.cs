using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Managers
{
    public abstract class NamedManager : MonoBehaviour
    {
        #region Unity Methods

        protected virtual void Awake()
        {
            name = GetType().Name;
        }

        private void OnValidate()
        {
            name = GetType().Name;
        }

        #endregion
    }
}
