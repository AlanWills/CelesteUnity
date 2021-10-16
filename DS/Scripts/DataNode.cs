using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.DS
{
    [Serializable]
    public abstract class DataNode : Node
    {
        #region Unity Methods

        protected override void Init()
        {
            base.Init();

            hideFlags = HideFlags.HideInHierarchy;
        }

        #endregion

        #region Add/Remove/Copy

        public void AddToGraph()
        {
            OnAddToGraph();
        }

        protected virtual void OnAddToGraph() { }

        public void RemoveFromGraph()
        {
            OnRemoveFromGraph();
        }

        protected virtual void OnRemoveFromGraph() { }

        public void CopyInGraph(DataNode original)
        {
            OnCopyInGraph(original);
        }

        protected virtual void OnCopyInGraph(DataNode original) { }

        #endregion
    }
}
