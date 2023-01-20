namespace Celeste.Objects
{
    public interface ICopyable<T>
    {
        void CopyFrom(T original);
    }
}
