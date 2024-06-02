namespace Celeste.Components
{
    public interface IComponentContainerUsingTemplates<T> : IComponentContainer<T> where T : BaseComponent
    {
        void SetComponentData(int index, ComponentData componentData);
        ComponentData GetComponentData(int index);
    }
}
