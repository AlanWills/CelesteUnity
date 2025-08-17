namespace Celeste.Components
{
    public interface IComponentContainer<T> where T : BaseComponent
    {
        int NumComponents { get; }

        T GetComponent(int index);
        void RemoveComponent(int componentIndex);
        bool HasComponent<K>() where K : T;
        K FindComponent<K>() where K : T;
    }
}
