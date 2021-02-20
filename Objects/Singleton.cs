using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Objects
{
    [Serializable]
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        #region Properties and Fields

        public static T Instance { get; private set; }

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            Debug.AssertFormat(Instance == null, "{0} instance is already set", typeof(T).Name);
            Instance = this as T;
            Debug.AssertFormat(Instance != null, "{0} instance is not set", typeof(T).Name);
        }

        #endregion
    }
}
