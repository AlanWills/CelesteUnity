using Celeste.Utils;
using System;
using Celeste.Scene.Hierarchy;
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

        [Input] public string gameObjectName;
        [Output] public GameObject gameObject;

        [SerializeField] private StringGameObjectDictionary gameObjectCache; 
        [SerializeField] private FindConstraint findConstraint = FindConstraint.ActiveInHierarchy;
        [SerializeField] private bool cache = true;

        private GameObject foundGameObject;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            if (!cache || foundGameObject == null)
            {
                string _childName = GetInputValue(nameof(gameObjectName), gameObjectName);
                if (gameObjectCache == null || !gameObjectCache.TryFindItem(_childName, out foundGameObject))
                {
                    string[] splitChildName = _childName.Split('.');
                    foundGameObject = GameObjectExtensions.FindGameObject(splitChildName, findConstraint);
                }
            }

            Debug.AssertFormat(foundGameObject != null, "Could not find child '{0}'", gameObjectName);
            return foundGameObject;
        }

        #endregion
    }
}
