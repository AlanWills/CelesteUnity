namespace Celeste.Web
{
    public interface INetworkMessageHandler
    {
        void SetClient(INetworkingClient networkingClient);
        void SetServer(INetworkingServer networkingServer);
    }
}