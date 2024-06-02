using UnityEngine.Events;

namespace Celeste.Components
{
    public interface IComponentContainerRuntime<T> where T : BaseComponent
    {
        UnityEvent ComponentDataChanged { get; }

        int NumComponents { get; }
        IComponentContainerController<IComponentContainerRuntime<T>, T> Controller { get; set; }

        void InitializeComponents(IComponentContainerUsingSubAssets<T> componentContainer);
        void InitializeComponents(IComponentContainerUsingTemplates<T> componentContainer);
        ComponentHandle<T> GetComponent(int index);
        void AddComponent(ComponentHandle<T> componentHandle);
        void AddComponent(T component);
        void RemoveComponent(int componentIndex);
        bool HasComponent<K>() where K : class;
        bool TryFindComponent<K>(out InterfaceHandle<K> iFace) where K : class;
        bool TryFindComponent<K>(out ComponentHandle<K> component) where K : T;
    }
}
