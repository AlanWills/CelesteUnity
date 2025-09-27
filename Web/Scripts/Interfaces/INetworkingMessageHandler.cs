using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingMessageHandler
    {
        bool CanHandle(NetworkingMessage message);
        void Handle(NetworkingMessage message);
    }
}