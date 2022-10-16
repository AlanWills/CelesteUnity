using Celeste.Loading;
using Celeste.Loading.Events;
using UnityEngine;

namespace Celeste.Bootstrap
{
    [AddComponentMenu("Celeste/Bootstrap/Bootstrap Manager")]
    public class BootstrapManager : MonoBehaviour
    {
        #region Properties and Fields

        public LoadJob bootstrapJob;
        public LoadJobEvent executeLoadJob;

        #endregion

        public void Start()
        {
            executeLoadJob.Invoke(bootstrapJob);
        }
    }
}
