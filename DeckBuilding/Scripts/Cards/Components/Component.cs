using System;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [Serializable]
    public struct ComponentHandle
    {
        public bool IsValid
        {
            get { return component != null; }
        }

        public Component component;
        public Instance instance;

        public ComponentHandle(Component component, ComponentData data, ComponentEvents events)
        {
            this.component = component;
            instance = new Instance(data, events);
        }

        public bool SupportsInterface<T>()
        {
            return component is T;
        }

        public bool Is<T>() where T : Component
        {
            return component is T;
        }

        public ComponentHandle<T> As<T>() where T : Component
        {
            return new ComponentHandle<T>(component as T, instance);
        }
    }

    [Serializable]
    public struct ComponentHandle<TComponent> where TComponent : Component
    {
        public bool IsValid
        {
            get { return component != null; }
        }

        public TComponent component;
        public Instance instance;

        public ComponentHandle(TComponent component, Instance instance)
        {
            this.component = component;
            this.instance = instance;
        }
    }

    [Serializable]
    public class ComponentData 
    {
        [NonSerialized] public CardRuntime Card;

        public string ToJson() { return JsonUtility.ToJson(this); }
        public void FromJson(string json) { JsonUtility.FromJsonOverwrite(json, this); }
    }

    public class ComponentEvents { }

    public struct Instance
    {
        public ComponentData data;
        public ComponentEvents events;

        public Instance(ComponentData data, ComponentEvents events)
        {
            this.data = data;
            this.events = events;
        }
    }

    public abstract class Component : ScriptableObject
    {
        public virtual ComponentData CreateData()
        {
            return new ComponentData();
        }

        public virtual ComponentEvents CreateEvents()
        {
            return new ComponentEvents();
        }
    }
}