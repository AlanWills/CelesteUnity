using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Application
{
    [AddComponentMenu("Celeste/Application/Internet Reachability")]
    public class InternetReachability : MonoBehaviour
    {
        #region Properties and Fields

        public BoolValue hasInternetConnection;

        #endregion

        #region Unity Methods

        private void Update()
        {
            hasInternetConnection.Value = UnityEngine.Application.internetReachability != NetworkReachability.NotReachable;
        }

        #endregion
    }
}
