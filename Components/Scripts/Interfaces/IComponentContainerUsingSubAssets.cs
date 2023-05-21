using System;

namespace Celeste.Components
{
    public interface IComponentContainerUsingSubAssets<T> : IComponentContainer<T> where T : Component
    {
        K CreateComponent<K>() where K : T;
        T CreateComponent(Type type);
    }
}
