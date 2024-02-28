using Celeste.Objects;
using UnityEngine;
using XNode;

namespace Celeste.DS
{
    [CreateAssetMenu(
        fileName = "DataGraph", 
        menuName = CelesteMenuItemConstants.DS_MENU_ITEM + "Data Graph",
        order = CelesteMenuItemConstants.DS_MENU_ITEM_PRIORITY)]
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
