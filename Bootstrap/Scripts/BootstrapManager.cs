using Celeste.Loading;
using Celeste.Loading.Events;
using UnityEngine;

namespace DnD.Bootstrap
{
    [AddComponentMenu("DnD/Bootstrap/Bootstrap Manager")]
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
