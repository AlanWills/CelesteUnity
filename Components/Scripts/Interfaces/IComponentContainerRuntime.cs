using UnityEngine.Events;

namespace Celeste.Components
{
    public interface IComponentContainerRuntime<T> where T : Component
    {
        UnityEvent ComponentDataChanged { get; }

        int NumComponents { get; }

        void InitializeComponents(IComponentContainer<T> componentContainer);
        ComponentHandle<T> GetComponent(int index);
        void AddComponent(ComponentHandle<T> componentHandle);
        void AddComponent(T component);
        void RemoveComponent(int componentIndex);
        bool HasComponent<K>() where K : T;
        bool TryFindComponent<K>(out InterfaceHandle<K> iFace) where K : class;
    }
}
