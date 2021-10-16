using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Results
{
    public abstract class LoseCondition : ScriptableObject
    {
        public UnityEvent OnLose { get; private set; }

        public void Hookup(UnityAction onLose)
        {
            OnLose = new UnityEvent();
            OnLose.AddListener(onLose);
            OnHookup();
        }

        public void Release()
        {
            OnLose.RemoveAllListeners();
            OnRelease();
        }

        protected abstract void OnHookup();
        protected abstract void OnRelease();

        public virtual void OnCardRemovedFromStage(CardRuntime card) { }
    }
}