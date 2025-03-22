using Celeste.FSM;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    public abstract class NarrativeView : MonoBehaviour
    {
        public virtual void OnNarrativeBegin() { }
        public virtual void OnNarrativeEnd() { }
        
        public abstract bool IsValidForNode(FSMNode fsmNode);
        public abstract void OnNodeEnter(FSMNode fsmNode);
        public abstract void OnNodeUpdate(FSMNode fsmNode);
        public abstract void OnNodeExit(FSMNode fsmNode);
    }
}
