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
    [CreateNodeMenu("Celeste/Objects/Get Child")]
    public class GetChildNode : DataNode
    {
        #region Properties and Fields

        [Input]
        public int childIndex;

        [Output]
        public GameObject child;

        private GameObject gameObject;
        private GameObject foundChild;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            if (foundChild == null)
            {
                int _childIndex = GetInputValue("childIndex", childIndex);
                foundChild = gameObject.transform.GetChild(_childIndex).gameObject;
            }

            return foundChild;
        }

        #endregion
    }
}
