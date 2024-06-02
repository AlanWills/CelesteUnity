using System;

namespace Celeste.Components
{
    public interface IComponentContainerUsingSubAssets<T> : IComponentContainer<T> where T : BaseComponent
    {
        K CreateComponent<K>() where K : T;
        T CreateComponent(Type type);
    }
}
