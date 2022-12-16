using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Components
{
    [Serializable]
    public struct InterfaceHandle<T> where T : class
    {
        public static readonly InterfaceHandle<T> NULL = new InterfaceHandle<T>();

        public bool IsValid => iFace != null;

        public T iFace;
        public Instance instance;

        public InterfaceHandle(T iFace, Instance instance)
        {
            this.iFace = iFace;
            this.instance = instance;
        }

        public void MakeNull()
        {
            iFace = null;
            instance.MakeNull();
        }
    }

    [Serializable]
    public struct ComponentHandle
    {
        public static readonly ComponentHandle NULL = new ComponentHandle();

        public bool IsValid => component != null;

        public Component component;
        public Instance instance;

        public ComponentHandle(Component component, ComponentData data, ComponentEvents events)
             : this(component, new Instance(data, events))
        {
        }

        public ComponentHandle(Component component, Instance instance)
        {
            this.component = component;
            this.instance = instance;
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
        public static readonly ComponentHandle<TComponent> NULL = new ComponentHandle<TComponent>();
        
        public bool IsValid => component != null;

        public TComponent component;
        public Instance instance;

        public ComponentHandle(TComponent component, ComponentData data, ComponentEvents events)
            : this(component, new Instance(data, events))
        {
        }

        public ComponentHandle(TComponent component, Instance instance)
        {
            this.component = component;
            this.instance = instance;
        }

        public bool Is<K>() where K : class
        {
            return component is K;
        }

        public InterfaceHandle<K> AsInterface<K>() where K : class
        {
            return new InterfaceHandle<K>(component as K, instance);
        }

        public ComponentHandle<K> AsComponent<K>() where K : TComponent
        {
            return new ComponentHandle<K>(component as K, instance);
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

        public void MakeNull()
        {
            data = null;
            events = null;
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

        public virtual void Load(Instance instance) { }
        public virtual void Shutdown(Instance instance) { }
    }
}