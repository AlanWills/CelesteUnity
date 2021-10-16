using Celeste.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Celeste.FSM.Nodes.Game
{
    [CreateNodeMenu("Celeste/Game/Quit Game")]
    [NodeTint(0.2f, 0.2f, 0.6f)]
    public class QuitGameNode : FSMNode
    {
        #region FSM Runtime Overrides

        protected override void OnEnter()
        {
            base.OnEnter();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

#endregion
    }
}
