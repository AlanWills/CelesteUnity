using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.DS
{
    [CreateAssetMenu(fileName = "DataGraph", menuName = "Celeste/DS/Data Graph")]
    public class DataGraph : NodeGraph
    {
        #region Data Updating

        public void Initialize(GameObject gameObject)
        {
            foreach (Node node in nodes)
            {
                if (node is IRequiresGameObject)
                {
                    (node as IRequiresGameObject).GameObject = gameObject;
                }
            }
        }

        public void Update()
        {
            foreach (Node node in nodes)
            {
                if (node is IUpdateable)
                {
                    (node as IUpdateable).Update();
                }
            }
        }

        #endregion
    }
}
