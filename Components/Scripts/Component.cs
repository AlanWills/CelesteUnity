using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Components
{
    [Serializable]
    public struct InterfaceHandle<T> where T : class
    {
        public bool IsValid
        {
            get { return iFace != null; }
        }

        public T iFace;
        public Instance instance;

        public InterfaceHandle(T iFace, Instance instance)
        {
            this.iFace = iFace;
            this.instance = instance;
        }
    }

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

        public bool Is<T>()
        {
            return component is T;
        }

        public InterfaceHandle<T> AsInterface<T>() where T : class
        {
            return new InterfaceHandle<T>(component as T, instance);
        }

        public ComponentHandle<T> AsComponent<T>() where T : Component
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
        public string ToJson() { return JsonUtility.ToJson(this); }
        public void FromJson(string json) { JsonUtility.FromJsonOverwrite(json, this); }
    }

    public class ComponentEvents 
    {
        public UnityEvent ComponentDataChanged { get; } = new UnityEvent();
    }

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

    public class Component : ScriptableObject
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