using Celeste.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using XNodeEditor;

namespace CelesteEditor.DS
{
    [CustomNodeGraphEditor(typeof(DataGraph))]
    public class DataGraphEditor : NodeGraphEditor
    {
        #region Context Menu

        public override string GetNodeMenuName(Type type)
        {
            return typeof(DataNode).IsAssignableFrom(type) ? base.GetNodeMenuName(type) : null;
        }

        #endregion
    }
}
