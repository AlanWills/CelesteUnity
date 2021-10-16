using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Celeste.FSM
{
    [Serializable]
    public class FSMNodeUnityEvent : UnityEvent<FSMNode>
    {
    }
}
