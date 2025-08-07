namespace Celeste.Events
{
    public interface IEventListener
    {
        void OnEventRaised();
    }

    public interface IEventListener<in T>
    {
        void OnEventRaised(T arguments);
    }
}
