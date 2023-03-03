using Celeste.Utils;
using System;
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
        public string gameObjectName;

        public FindConstraint findConstraint = FindConstraint.ActiveInHierarchy;
        public bool cache = true;

        [Output]
        public GameObject gameObject;

        private GameObject foundGameObject;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            if (!cache || foundGameObject == null)
            {
                string _childName = GetInputValue(nameof(gameObjectName), gameObjectName);
                string[] splitChildName = _childName.Split('.');
                foundGameObject = GameObjectExtensions.FindGameObject(splitChildName, findConstraint);
            }

            Debug.AssertFormat(foundGameObject != null, "Could not find child '{0}'", gameObjectName);
            return foundGameObject;
        }

        #endregion
    }
}
