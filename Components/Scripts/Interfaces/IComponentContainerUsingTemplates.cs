namespace Celeste.Components
{
    public interface IComponentContainerUsingTemplates<T> : IComponentContainer<T> where T : Component
    {
        void SetComponentData(int index, ComponentData componentData);
    }
}
