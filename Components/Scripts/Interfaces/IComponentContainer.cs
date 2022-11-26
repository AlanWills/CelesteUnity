using System;

namespace Celeste.Components
{
    public interface IComponentContainer<T> where T : Component
    {
        int NumComponents { get; }

        T GetComponent(int index);
        void CreateComponent<K>() where K : T;
        void CreateComponent(Type type);
        void RemoveComponent(int componentIndex);
        bool HasComponent<K>() where K : T;
    }
}
