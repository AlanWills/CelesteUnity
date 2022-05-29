using System;

namespace Celeste.Components
{
    public interface IComponentContainer
    {
        int NumComponents { get; }

        Component GetComponent(int index);
        void AddComponent<T>(T component) where T : Component;
        void RemoveComponent(int componentIndex);
        bool HasComponent<T>() where T : Component;
    }
}
