using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.DS.Nodes.Objects
{
    [Serializable]
    [CreateNodeMenu("Celeste/Objects/Active Controller")]
    public class ActiveControllerNode : DataNode, IUpdateable
    {
        #region Properties and Fields

        [Input]
        public bool isActive;

        [Input]
        public GameObject gameObject;

        #endregion

        #region IUpdateable

        public void Update()
        {
            bool _isActive = GetInputValue(nameof(isActive), isActive);
            GameObject _gameObject = GetInputValue(nameof(gameObject), gameObject);
            _gameObject.SetActive(_isActive);
        }

        #endregion
    }
}
