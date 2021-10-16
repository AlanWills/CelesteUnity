using Celeste.Utils;
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
    [CreateNodeMenu("Celeste/Objects/Find Game Object")]
    public class FindGameObjectNode : DataNode, IRequiresGameObject
    {
        #region Properties and Fields

        public GameObject GameObject { get; set; }

        [Input]
        public string childName;

        public FindConstraint findConstraint = FindConstraint.ActiveInHierarchy;
        public bool cache = true;

        [Output]
        public GameObject child;

        private GameObject foundChild;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            if (!cache || foundChild == null)
            {
                string _childName = GetInputValue(nameof(childName), childName);
                string[] splitChildName = _childName.Split('.');
                foundChild = GameObjectUtils.FindGameObject(splitChildName, findConstraint);
            }

            Debug.AssertFormat(foundChild != null, "Could not find child '{0}'", childName);
            return foundChild;
        }

        #endregion
    }
}
