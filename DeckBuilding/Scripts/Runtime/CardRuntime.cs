using Celeste.Components;
using Celeste.Constants;
using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.Events;
using System;
using System.Collections.Generic;

namespace Celeste.DeckBuilding
{
    [Serializable]
    public class CardRuntime
    {
        #region Events

        public BoolUnityEvent OnFaceUpChanged { get; } = new BoolUnityEvent();
        public BoolUnityEvent OnCanPlayChanged { get; } = new BoolUnityEvent();
        public PlayCardSuccessUnityEvent OnPlayCardSuccess { get; } = new PlayCardSuccessUnityEvent();
        public PlayCardFailureUnityEvent OnPlayCardFailure { get; } = new PlayCardFailureUnityEvent();

        #endregion

        #region Properties and Fields

        public ID Owner { get; set; }

        public int CardGuid => card.Guid;
        public string CardName => card.name;

        public bool IsFaceUp
        {
            get { return faceUp; }
            set
            {
                if (faceUp != value)
                {
                    faceUp = value;
                    OnFaceUpChanged.Invoke(faceUp);
                }
            }
        }

        public bool CanPlay
        {
            get { return canPlay; }
            set
            {
                if (canPlay != value)
                {
                    canPlay = value;
                    OnCanPlayChanged.Invoke(canPlay);
                }
            }
        }

        public int NumComponents 
        {
            get { return components.Count; }
        }

        [NonSerialized] private Card card;
        [NonSerialized] private List<ComponentHandle> components = new List<ComponentHandle>();
        [NonSerialized] private bool faceUp = false;
        [NonSerialized] private bool canPlay;

        #endregion

        public CardRuntime(Card card)
        {
            this.card = card;

            InitComponents();
        }

        #region Components

        private void InitComponents()
        {
            for (int i = 0, n = card.NumComponents; i < n; ++i)
            {
                AddComponent(card.GetComponent(i));
            }
        }

        public void LoadComponents(List<string> componentNames, List<string> componentData)
        {
            for (int i = 0, n = componentNames.Count; i < n; ++i)
            {
                string componentName = componentNames[i];
                var existingComponent = components.Find(x => string.CompareOrdinal(x.component.GetType().Name, componentName) == 0);

                if (existingComponent.IsValid)
                {
                    existingComponent.instance.data.FromJson(componentData[i]);
                }
                else
                {
                    // Maybe re-add this later if we add dynamic components, but for now any missing component is ignored
                    UnityEngine.Debug.LogAssertion($"Ignoring component '{componentName}' from save file as it is not present on card {card.name}.");
                    //Cards.Component component = ScriptableObject.CreateInstance(componentName) as Cards.Component;
                    //UnityEngine.Debug.Assert(component != null, $"Unable to create component {componentName} from save data.");
                    //existingComponent = AddComponent(component);
                }
            }
        }

        private ComponentHandle AddComponent(Components.Component component)
        {
            ComponentData data = component.CreateData();
            ComponentEvents events = component.CreateEvents();
            ComponentHandle handle = new ComponentHandle(component, data, events);
            components.Add(handle);

            return handle;
        }

        public bool HasComponent<T>() where T : Components.Component
        {
            for (int i = 0, n = components.Count; i < n; ++i)
            {
                if (components[i].Is<T>())
                {
                    return true;
                }
            }

            return false;
        }

        public ComponentHandle<T> FindComponent<T>() where T : Components.Component
        {
            for (int i = 0, n = components.Count; i < n; ++i)
            {
                if (components[i].Is<T>())
                {
                    return components[i].AsComponent<T>();
                }
            }

            return new ComponentHandle<T>();
        }

        public ComponentHandle GetComponent(int index)
        {
            return components.Get(index);
        }

        #endregion

        public bool IsForCard(Card card)
        {
            return this.card == card;
        }

        public bool TryPlay()
        {
            if (CanPlay)
            {
                OnPlayCardSuccess.Invoke(new PlayCardSuccessArgs() { cardRuntime = this });
                return true;
            }
            else
            {
                OnPlayCardFailure.Invoke(new PlayCardFailureArgs() { cardRuntime = this });
                return false;
            }
        }
    }
}