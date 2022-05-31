using Celeste.Components;
using System.Collections;

namespace Celeste.LiveOps
{
    public interface IRequiresAssets
    {
        IEnumerator Load(InterfaceHandle<ILiveOpAssets> assets);
    }
}
