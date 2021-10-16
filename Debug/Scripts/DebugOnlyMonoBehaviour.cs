using Celeste.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Debug
{
    [AddComponentMenu("Celeste/Debug/Debug Only")]
    public class DebugOnlyMonoBehaviour : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField]
        private BoolValue isDebugBuild;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (isDebugBuild == null || !isDebugBuild.Value)
            {
                UnityEngine.Debug.LogFormat("Destroying {0} due to unset or false value IsDebugBuild parameter", gameObject.name);
                GameObject.Destroy(gameObject);
            }
        }

        #endregion
    }
}
