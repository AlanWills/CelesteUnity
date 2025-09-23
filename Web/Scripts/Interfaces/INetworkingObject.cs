namespace Celeste.Web
{
    public interface INetworkingObject
    {
        bool Exists { get; }
        bool HasNetworkObject { get; }
    }
}