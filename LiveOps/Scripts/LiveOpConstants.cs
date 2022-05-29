using Celeste.Components;

namespace Celeste.LiveOps
{
    public static class LiveOpConstants
    {
        public static InterfaceHandle<ILiveOpTimer> NO_TIMER = new InterfaceHandle<ILiveOpTimer>()
        {
            iFace = new NoLiveOpTimer(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static InterfaceHandle<ILiveOpProgress> NO_PROGRESS = new InterfaceHandle<ILiveOpProgress>()
        {
            iFace = new NoLiveOpProgress(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };
    }
}
